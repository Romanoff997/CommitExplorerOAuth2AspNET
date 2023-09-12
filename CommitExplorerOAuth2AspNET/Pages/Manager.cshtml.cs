using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Models.Pager;
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
        public async Task<IActionResult> Pager(int page=1)
        {
            
            if (commits.Count > 0)
            {
                const int pageSize = 5;
                if (page < 1)
                    page = 1;
                int recsCount = await GetTotalCount();
                var pager = new Pager(recsCount, page, pageSize);
                int recSkip = (page - 1) * pageSize;
                var data = requestGetClients.data.Skip(recSkip).Take(pager.PageSize).ToList();
            }
                return Partial("TableCommitsPartial", commit);
        }
        private async Task<int> GetTotalCount()
        {
            return await _gitHubController.GetTotalCount(owner, repo);
        }
        private async Task<List<GitCommit>> GetCommits(int page=1)
        {
            return await _gitHubController.GetCommits(owner, repo);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetFromDb()
        {
            return Partial("TableCommitsPartial", await GetCommits());
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            await _gitHubController.UpdateCommits(User, owner, repo);
            commits = await GetCommits();
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteDb(string[] deleteId)
        {
            await _gitHubController.DeleteCommits(deleteId.ToList(), owner, repo);
            return Partial("TableCommitsPartial", await GetCommits());
        }

        [BindProperty(SupportsGet = true)]
        public Pager pager { get; set; }

        [BindProperty(SupportsGet = true)]
        public string owner { get; set; }

        [BindProperty(SupportsGet = true)]
        public string repo { get; set; }

        [BindProperty]
        public List<GitCommit> commits { get; set; } = new();
    }
}
