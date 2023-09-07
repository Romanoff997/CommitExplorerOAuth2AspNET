
using Microsoft.EntityFrameworkCore;
using CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;
using CommitExplorerOAuth2AspNET.Shared.Interface;

namespace SimplifyLink.Domain.Repositories.EntityFramework
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
        public async Task<List<GitCommit>> GetCommits(List<GitHubCommit> commits, string owner, string repo)
        {
            var user = await AddUser(owner);

            var repository = await AddRepository(owner, user);

            var commitsList = await AddCommits(repository, commits);

            await _context.SaveChangesAsync();

            return commitsList;
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
            List<GitCommit> commits = repository.GitCommits?.ToList();
            if (commits == null)
            {
                commits = gitHubCommits.Select(x => new GitCommit()
                {
                    GitRepository = repository,
                    GitRepositoryId = repository.Id,
                    
                    author = x.Author.Login,
                    sha = x.Sha,

                    json = _converter.WriteJson(x)
                }).ToList();
                await _context.CommitEntity.AddRangeAsync(commits);
            }
            return commits;
        }
    }
}
