using Octokit;

namespace CommitExplorerOAuth2AspNET.Models
{
    public class GitCommitsViewModel
    {
        public List<GitHubCommit> data { get; set; }
        public int TotalCount { get; set; }
        public int page { get; set; }
    }
}
