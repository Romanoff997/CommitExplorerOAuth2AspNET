using Octokit;
using CommitExplorerOAuth2AspNET.Models.Pager;
using CommitExplorerOAuth2AspNET.Domain.Entities;

namespace CommitExplorerOAuth2AspNET.Models
{
    public class GitCommitsViewModel
    {
        public IEnumerable<GitCommit> data { get; set; }
        public ListPager pager { get; set; }
    }
}
