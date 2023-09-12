using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class GitHubController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly GitHubService _gitHubService;

        public GitHubController(DataManager dataManager, GitHubService gitHubService)
        {
            _dataManager = dataManager;
            _gitHubService = gitHubService;
        }
        public async Task UpdateCommits(ClaimsPrincipal user, string owner, string repo)
        {
            var commits = await _gitHubService.GetCommits(user, owner, repo);
            await _dataManager.CommitRepository.UpdateCommits(commits, owner, repo);
        }
        public async Task<int> GetTotalCount(string owner, string repo)
        {
            return await _dataManager.CommitRepository.GetTotalCount(owner, repo);
        }
        public async Task<List<GitCommit>> GetCommits(string owner, string repo, int page, int pageSize)
        {
            return await _dataManager.CommitRepository.GetCommits(owner, repo, page, pageSize);
        }
        public async Task DeleteCommits(List<string> deleteId, string owner, string repo)
        {
            await _dataManager.CommitRepository.DeleteCommits(deleteId, owner, repo);
        }
        public IActionResult SignOut()
        {
            return Redirect("/signout");
        }
        public IActionResult SignIn()
        {
            return Redirect("/signin");
        }
        public IActionResult Account()
        {
            return RedirectToPage("/account");
        }


        }
}
