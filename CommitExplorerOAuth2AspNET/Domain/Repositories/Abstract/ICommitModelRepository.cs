using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract
{
    public interface ICommitModelRepository
    {
        public Task UpdateCommits(List<GitHubCommit> commits, string owner, string repo);
        public Task DeleteCommits(List<string> deleteSha, string owner, string repo);
        public Task<List<GitCommit>> GetCommits(string owner, string repo, int page, int count);
        public Task<int> GetTotalCount(string owner, string repo);
    }
}
