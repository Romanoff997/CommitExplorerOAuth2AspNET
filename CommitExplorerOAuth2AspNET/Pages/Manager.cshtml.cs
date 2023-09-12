using Azure;
using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Models.Pager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;

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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetPager(int pg, string owner, string repo)
        {
            this.owner = owner;
            this.repo = repo;
            if (pg>0)
            {
                await NewCommit(pg);
            }
            return Partial("TableCommitsPartial", commits);
        }
        private async Task<int> GetTotalCount()
        {
            return await _gitHubController.GetTotalCount(owner, repo);
        }
        private async Task<List<GitCommit>> GetCommits(int page=1)
        {
            return await _gitHubController.GetCommits(owner, repo, page);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetFromDb()
        {
            commits.data = await GetCommits();
            return Partial("TableCommitsPartial", commits);
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost()
        {
            await _gitHubController.UpdateCommits(User, owner, repo);
            await NewCommit();
        }

        public async Task NewCommit(int page=1)
        {
            const int pageSize = 5;
            int recsCount = await GetTotalCount();
            commits.pager = new ListPager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            commits.data = await GetCommits(recSkip);
        }
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteDb(string[] deleteId)
        {
            await _gitHubController.DeleteCommits(deleteId.ToList(), owner, repo);
            await NewCommit();
            return Partial("TableCommitsPartial", commits);
        }



        [BindProperty(SupportsGet = true)]
        public string owner { get; set; }

        [BindProperty(SupportsGet = true)]
        public string repo { get; set; }

        [BindProperty(SupportsGet = true)]
        public GitCommitsViewModel commits { get; set; } = new();
    }
}
