using Octokit;
using CommitExplorerOAuth2AspNET.Models;
using System.Security.Claims;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using Azure.Core;

namespace CommitExplorerOAuth2AspNET.Service
{
    public class GitHubService
    {
        private readonly MyConfiguration _gitConfiguration;
        public GitHubService(MyConfiguration gitConfiguration) 
        {
            _gitConfiguration = gitConfiguration;
        }

        public async Task<bool> CheckUser(ClaimsPrincipal user)
        {
            if (GetAccessToken(user) is { } accessToken)
            {
                var github = new GitHubClient(new ProductHeaderValue("MyApp"));
                github.Credentials = new Credentials(accessToken);

                try
                {
                    await github.User.Current();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new AuthorizationException();
                }
            }
            return false;
        }


        public async Task<IReadOnlyList<GitHubCommit>> GetCommits(ClaimsPrincipal user, string owner, string repo)
        {
            try
            {
                if (GetAccessToken(user) is { } accessToken)
                {
                    var github = new GitHubClient(new ProductHeaderValue("c"))
                    {
                        Credentials = new Credentials(accessToken)
                    };
                    var result = await github.Repository.Commit.GetAll(owner, repo);
                    return result;
                }
                throw new Exception();
            }        
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении коммитов: " + ex.Message);
                throw new AuthorizationException();
            }
        }


        public string GetAccessToken(ClaimsPrincipal user)
        {
            return user.AccessToken(_gitConfiguration.tokenName);
        }
    }
}
