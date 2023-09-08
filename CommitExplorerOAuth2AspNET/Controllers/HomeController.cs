using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;

namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class HomeController : Controller
    {
 
        private  GitHubService _gitHubService;
        public HomeController( GitHubService gitHubService)
        {
            //_logger = logger;
            _gitHubService = gitHubService;
        }

        public async Task<IActionResult> Index()
        {
            if (_gitHubService.GetAccessToken(User) is { })
            {
                return RedirectToPage("/Manager");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}