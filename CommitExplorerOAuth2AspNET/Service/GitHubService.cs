using Octokit;
using CommitExplorerOAuth2AspNET.Models;
using System.Security.Claims;

namespace CommitExplorerOAuth2AspNET.Service
{
    public class GitHubService
    {
        private readonly MyConfiguration _gitConfiguration;
        public event Action disconect;
        public GitHubService(MyConfiguration gitConfiguration) 
        {
            _gitConfiguration = gitConfiguration;
        }

        public async Task<List<GitHubCommit>> GetCommits(ClaimsPrincipal user, string owner, string repo)
        {
            if (GetAccessToken(user) is { } accessToken)
            {
                var github = new GitHubClient(new ProductHeaderValue("MyApp"))
                {
                    Credentials = new Credentials(accessToken)
                };

                try
                {
                    var result = await github.Repository.Commit.GetAll(owner, repo);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    disconect?.Invoke();
                }
            }
            return new List<GitHubCommit>();
        }


        public string GetAccessToken(ClaimsPrincipal user)
        {
            return user.AccessToken(_gitConfiguration.tokenName);
        }
    }
}
