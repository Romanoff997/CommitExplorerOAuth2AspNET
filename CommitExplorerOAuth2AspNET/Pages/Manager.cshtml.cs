using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;

namespace CommitExplorerOAuth2AspNET.Pages
{
    public class ManagerModel : PageModel
    {
        private readonly GitHubService _gitHubService;
        public ManagerModel(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }
        public void OnGet()
        {

        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            commits = await _gitHubService.GetCommits(User, owner, repo);
        }


        [BindProperty]
        public string owner { get; set; }

        [BindProperty]
        public string repo { get; set; }

        [BindProperty]
        public List<GitHubCommit> commits { get; set; } = new();
    }
}
