using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommitExplorerOAuth2AspNET.Pages
{
    public class ManagerModel : PageModel
    {
        //private readonly GitHubService _gitHubService;
        private readonly GitHubController _gitHubController;
        public ManagerModel(GitHubController gitHubController)
        {
            //_gitHubService = gitHubService;
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
            return Partial("TableCommitsPartial", await GetCommits("Romanoff997", "SignalRWebApi"));
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            
            await _gitHubController.UpdateCommits(User,"Romanoff997", "SignalRWebApi");

            commits = await GetCommits("Romanoff997", "SignalRWebApi");
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteDb(string[] deleteId)
        {
            await _gitHubController.DeleteCommits(deleteId.ToList(), "Romanoff997", "SignalRWebApi");
            return Partial("TableCommitsPartial", await GetCommits("Romanoff997", "SignalRWebApi"));

        }
        


        [BindProperty ]
        public string owner { get; set; }

        [BindProperty]
        public string repo { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<GitCommit> commits { get; set; } = new();
    }
}
