using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract
{
    public interface ICommitModelRepository
    {
        public Task UpdateCommits(IEnumerable<GitHubCommit> commits, string owner, string repo);
        public Task DeleteCommits(IEnumerable<string> deleteSha, string owner, string repo);
        public Task<IEnumerable<GitCommit>> GetCommits(string owner, string repo, int page, int pageSize);
        public Task<int> GetTotalCount(string owner, string repo);
    }
}
