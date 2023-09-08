
using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Interface;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Security.Claims;

namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class GitHubController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly GitHubService _gitHubService;
        public GitHubController(DataManager dataManager, GitHubService gitHubService, IMapingService mapper )
        {
            _dataManager = dataManager;
            _gitHubService = gitHubService;

            //_gitHubService.disconect = Disconect;
        }
        public async Task UpdateCommits(ClaimsPrincipal user, string owner, string repo)
        {
            var commits = await _gitHubService.GetCommits(user, owner, repo);
            await _dataManager.CommitRepository.UpdateCommits(commits, owner, repo);
        }
        public async Task<List<GitCommit>> GetCommits(string owner, string repo)
        {
            return await _dataManager.CommitRepository.GetCommits(owner, repo);
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
        private void Disconect()
        {
             Redirect("/signout");
        }

    }
}
