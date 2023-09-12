
using Microsoft.EntityFrameworkCore;
using CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;
using CommitExplorerOAuth2AspNET.Shared.Interface;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories.EntityFramework
{
    public class EFCommitModelRepository : ICommitModelRepository
    {
        private readonly MyDbContext _context;
        private readonly IJsonConverter _converter;
        public EFCommitModelRepository(MyDbContext context, IJsonConverter converter)
        {
            _context = context;
            _converter = converter;
        }
        public async Task<int> GetTotalCount(string owner, string repo)
        {
            var user = await _context.UserEntity.FirstOrDefaultAsync(user => user.Name == owner);
            if (user != null)
            {
                var repository = _context.RepoEntity.Include(x => x.GitCommits).FirstOrDefault(x => x.Name == repo && x.GitUser.Equals(user));
                int? count = repository?.GitCommits.Count;
                return count!=null? count.Value : 0;
            }

            return 0;
        }
        public async Task<List<GitCommit>> GetCommits(string owner, string repo, int page, int pageSize)
        {
            var user = await _context.UserEntity.FirstOrDefaultAsync(user => user.Name == owner);
            if (user != null)
            {
                var repository = _context.RepoEntity.Include(repository => repository.GitCommits).FirstOrDefault(x => x.Name == repo && x.GitUser.Equals(user));
                return repository?.GitCommits.Skip(page).Take(pageSize).ToList();
            }

            return null;
        }
        public async Task DeleteCommits(List<string> deleteId, string owner, string repo)
        {
            var deleteList = await _context.CommitEntity.Where(e => deleteId.Contains(e.Id.ToString())).ToListAsync();
            if (deleteList.Count > 0)
            {
                _context.CommitEntity.RemoveRange(deleteList.ToList());
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateCommits(List<GitHubCommit> commits, string owner, string repo)
        {
            var user = await AddUser(owner);

            var repository = await AddRepository(repo, user);

            var commitsList = await AddCommits(repository, commits);

            await _context.SaveChangesAsync();
        }
        public async Task<GitUser> AddUser(string owner)
        {
            GitUser user =   _context.UserEntity.FirstOrDefault(x => x.Name == owner);
            if (user == null)
            {
                user = new GitUser()
                {
                    Name = owner
                };
                await _context.UserEntity.AddAsync(user);
            }
            return user;
        }
            
        public async Task<GitRepository> AddRepository(string repo, GitUser user)
        {
            GitRepository repository = _context.RepoEntity.FirstOrDefault(x => x.Name == repo);
            if (repository == null)
            {
                repository = new GitRepository()
                {
                    Name = repo,
                    GitUser = user,
                    GitUserId = user.Id
                };
                await _context.RepoEntity.AddAsync(repository);
            }
            return repository;
        }            
        public async Task<List<GitCommit>> AddCommits(GitRepository repository, List<GitHubCommit> gitHubCommits)
        {
            List<GitCommit> currCommits =  _context.CommitEntity.Where(x => x.GitRepositoryId == repository.Id).ToList();
            var commits = gitHubCommits.Select(x => new GitCommit()
            {
                GitRepository = repository,
                GitRepositoryId = repository.Id,

                author = x.Author!=null ? x.Author.Login.ToString(): " ",
                sha = x.Sha,

                json = _converter.WriteJson(x)
            }).ToList();

            if (currCommits != null)
            {
                _context.CommitEntity.RemoveRange(currCommits);
            }

            await _context.CommitEntity.AddRangeAsync(commits);

            return commits;
        }
    }
}
