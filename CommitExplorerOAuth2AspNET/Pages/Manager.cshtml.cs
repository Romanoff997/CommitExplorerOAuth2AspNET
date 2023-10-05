using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Models.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CommitExplorerOAuth2AspNET.Pages
{
    [ValidateAntiForgeryToken]
    [Authorize]
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
 
        private async Task<int> GetTotalCount()
        {
            return await _gitHubController.GetTotalCount(owner, repo);
        }

        private async Task<List<GitCommit>> GetCommits(int page, int pageSize)
        {
            return await _gitHubController.GetCommits(owner, repo, page, pageSize);
        }

        private async Task InitGitCommitsViewModel(int page = 1)
        {
            int recsCount = await GetTotalCount();
            commits.pager = new ListPager(recsCount, page);
            var pageSize = commits.pager.PageSize;
            int recSkip = (page - 1) * pageSize;
            commits.data = await GetCommits(recSkip, pageSize);
        }
        
        public async Task<IActionResult> OnGetPager(int pg)
        {
            await InitGitCommitsViewModel(pg);
            return Partial("TableCommitsPartial", commits);
        }

        public async Task OnPost()
        {
            await _gitHubController.UpdateCommits(User, owner, repo);
            await InitGitCommitsViewModel();
        }

        public async Task<IActionResult> OnPostDeleteDb(string[] deleteId, int page)
        {
            await InitGitCommitsViewModel(page);
            var deleteList = commits.data.Where(e => deleteId.Contains(e.Id.ToString())).ToList() ;

            if (deleteList.Count > 0)
            {
                commits.data = commits.data.Except(deleteList).ToList();
                commits.pager = new ListPager(commits.pager.TotalItems- deleteId.Length, page);
            }
        
            await _gitHubController.DeleteCommits(deleteId.ToList(), owner, repo);
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
