﻿
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
namespace CommitExplorerOAuth2AspNET.Pages
{
    public class SignIn :PageModel
    {
        public IEnumerable<AuthenticationScheme> Schemes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public async Task OnGet()
        {
            Schemes = await GetExternalProvidersAsync(HttpContext);
        }

        public async Task<IActionResult> OnPost([FromForm] string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            return await IsProviderSupportedAsync(HttpContext, provider) is false
                ? BadRequest()
                : Challenge(new AuthenticationProperties
                {
                    RedirectUri = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/"
                }, provider);
        }

        private static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(HttpContext context)
        {
            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            return (await schemes.GetAllSchemesAsync())
                .Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName))
                .ToArray();
        }

        private static async Task<bool> IsProviderSupportedAsync(HttpContext context, string provider) =>
            (await GetExternalProvidersAsync(context))
            .Any(scheme => string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase));
    }
}

