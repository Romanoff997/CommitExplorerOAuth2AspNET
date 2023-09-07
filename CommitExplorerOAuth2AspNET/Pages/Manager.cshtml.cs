using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
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
        private readonly GitHubController _gitHubController;
        public ManagerModel(GitHubService gitHubService, GitHubController gitHubController)
        {
            _gitHubService = gitHubService;
            _gitHubController = gitHubController;
        }
        public void OnGet()
        {

        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            var gitHubCommits = await _gitHubService.GetCommits(User, owner, repo);
            commits = await _gitHubController.AddCommits(gitHubCommits, owner, repo);
        }


        [BindProperty]
        public string owner { get; set; }

        [BindProperty]
        public string repo { get; set; }

        [BindProperty]
        public List<GitCommit> commits { get; set; } = new();
    }
}
