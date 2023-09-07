
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Octokit;

namespace CommitExplorerOAuth2AspNET.Interface
{
    public interface IMapingService
    {
        public IQueryable<GitCommit> GetCommitsEntity(IQueryable<GitHubCommit> commitsView);

    }
}
