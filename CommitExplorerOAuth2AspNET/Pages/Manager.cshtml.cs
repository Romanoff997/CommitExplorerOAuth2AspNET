using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommitExplorerOAuth2AspNET.Pages
{
    public class ManagerModel : PageModel
    {

        private readonly GitHubController _gitHubController;
        public ManagerModel(GitHubController gitHubController)
        {
            _gitHubController = gitHubController;
        }
        public void OnGet()
        {

        }
        private async Task<List<GitCommit>> GetCommits(string owner, string repo)
        {
            return await _gitHubController.GetCommits(owner, repo);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetFromDb(string owner, string repo)
        {
            return Partial("TableCommitsPartial", await GetCommits(owner, repo));
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            await _gitHubController.UpdateCommits(User, owner, repo);
            commits = await GetCommits(owner, repo);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteDb(string[] deleteId, string owner, string repo)
        {
            await _gitHubController.DeleteCommits(deleteId.ToList(), owner, repo);
            return Partial("TableCommitsPartial", await GetCommits(owner, repo));
        }
        


        [BindProperty ]
        public string owner { get; set; }

        [BindProperty]
        public string repo { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<GitCommit> commits { get; set; } = new();
    }
}
