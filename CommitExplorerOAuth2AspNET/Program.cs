using AspNet.Security.OAuth.GitHub;
using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
ConfigurationManager configuration = builder.Configuration;
var gitConfig = configuration.GetSection("GitConfiguration").Get<GitConfiguration>(); 
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
        .AddCookie(o =>
        {
            o.LoginPath = "/signin";
            o.LogoutPath = "/signout";
        })
        .AddGitHub(o =>
        {
            o.ClientId = configuration["github:clientId"];
            o.ClientSecret = configuration["github:clientSecret"];
            o.CallbackPath = "/signin-github";
            o.Scope.Add("read:user");
            o.Events.OnCreatingTicket += context =>
            {
                if (context.AccessToken is { } token)
                {
                    context.Identity?.AddClaim(new Claim(gitConfig.tokenName, token));
                }

                return Task.CompletedTask;
            };
        });
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<MyDbContext>();
////builder.Services.AddTransient<ICityModelRepository, EFCityModelRepository>();
//builder.Services.AddTransient<DataManager>();
builder.Services.AddTransient<GitHubService>();
builder.Services.AddSingleton(gitConfig);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/signout", async ctx =>
    {
        await ctx.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    });
});

app.Run();
