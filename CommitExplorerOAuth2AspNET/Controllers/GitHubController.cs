
using CommitExplorerOAuth2AspNET.Domain.Entities;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Interface;
using Microsoft.AspNetCore.Mvc;
using Octokit;


namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class GitHubController : Controller
    {
        private readonly DataManager _dataManager;
        public GitHubController(DataManager dataManager, IMapingService mapper )
        {
            _dataManager = dataManager;
        }
        public async Task UpdateCommits(List<GitHubCommit> commits, string owner, string repo)
        {

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

    }
}
