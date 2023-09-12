using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract
{
    public interface ICommitModelRepository
    {
        public Task UpdateCommits(List<GitHubCommit> commits, string owner, string repo);
        public Task DeleteCommits(List<string> deleteSha, string owner, string repo);
        public Task<List<GitCommit>> GetCommits(string owner, string repo, int page, int pageSize);
        public Task<int> GetTotalCount(string owner, string repo);
    }
}
