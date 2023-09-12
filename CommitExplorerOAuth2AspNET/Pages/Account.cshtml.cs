using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Octokit;
using CommitExplorerOAuth2AspNET.Service;

namespace CommitExplorerOAuth2AspNET.Pages
{
    [Authorize]
    public class Account : PageModel
    {
        private readonly GitHubService _gitHubService;

        public Account(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }
        public async Task OnGet()
        {
            Claims = User.Claims.ToList();

            if (_gitHubService.GetAccessToken(User) is { } accessToken)
            {
                var client = new GitHubClient(new ProductHeaderValue("test"))
                {
                    Credentials = new Credentials(accessToken)
                };
                GitHubUser = await client.User.Get(User.Identity?.Name);
            }
            
        }

        public User GitHubUser { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
