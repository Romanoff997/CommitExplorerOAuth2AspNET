using Octokit;

namespace CommitExplorerOAuth2AspNET.Domain.Entities
{
    public class Commiter
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
    }

    public class Commit
    {
        public Commiter author { get; set; }
        public Commiter commiter { get; set; }
        public string message { get; set; }
    }

    public class CommitModel: GitHubCommit
    {
    }
}
