using CommitExplorerOAuth2AspNET.Models;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Mvc;

namespace CommitExplorerOAuth2AspNET.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identities.FirstOrDefault(x=>x.AuthenticationType=="GitHub") != null)
            {
                return RedirectToPage("/manager");
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