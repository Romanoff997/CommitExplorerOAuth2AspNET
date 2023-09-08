using CommitExplorerOAuth2AspNET.Controllers;
using CommitExplorerOAuth2AspNET.Domain.Repositories;
using CommitExplorerOAuth2AspNET.Domain.Repositories.Abstract;
using CommitExplorerOAuth2AspNET.Interface;
using CommitExplorerOAuth2AspNET.Mappings;
using CommitExplorerOAuth2AspNET.Service;
using CommitExplorerOAuth2AspNET.Services;
using CommitExplorerOAuth2AspNET.Shared.Interface;
using CommitExplorerOAuth2AspNET.Shared.Services;
using CRUDRazorWebAPI.Client.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SimplifyLink.Domain.Repositories.EntityFramework;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
ConfigurationManager configuration = builder.Configuration;
var appConfig = configuration.GetSection("AppConfiguration").Get<MyConfiguration>();

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
                    context.Identity?.AddClaim(new Claim(appConfig.tokenName, token));
                }

                return Task.CompletedTask;
            };
        });

builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(appConfig.connectionString));
builder.Services.AddTransient<GitHubController>();
builder.Services.AddTransient<ICommitModelRepository, EFCommitModelRepository>();
builder.Services.AddTransient<DataManager>();
builder.Services.AddTransient<GitHubService>();
builder.Services.AddTransient<IMapingService, MappingServiceNative>();
builder.Services.AddSingleton(appConfig);
builder.Services.AddSingleton<IJsonConverter>(json => new JsonNewtonConverter(new Newtonsoft.Json.JsonSerializerSettings()));

builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

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

