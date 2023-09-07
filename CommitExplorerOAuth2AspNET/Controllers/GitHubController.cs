
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
        private readonly IMapingService _mapper;
        public GitHubController(DataManager dataManager, IMapingService mapper )
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }
        public async Task<List<GitCommit>> AddCommits(List<GitHubCommit> commits, string owner, string repo)
        {

            return await _dataManager.CommitRepository.GetCommits(commits, owner, repo);
        }

    }
}
