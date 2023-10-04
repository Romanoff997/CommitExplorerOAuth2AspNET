using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly GitHubService _gitHubService;
        public HomeController(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        public async Task<IActionResult> Index()
        {
            var user = User.Identities.FirstOrDefault(x => x.AuthenticationType == "GitHub");
            if (user!=null && await _gitHubService.CheckUser(new ClaimsPrincipal(user)))
            {
                return RedirectToPage("/manager");
            }
            return Redirect("/signin");
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