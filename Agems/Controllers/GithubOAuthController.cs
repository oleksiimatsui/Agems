using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using Octokit.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Agems
{
    
    public class GithubOAuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string url)
        {
            url = url.Replace('/', '*');
            string returnUrl = "/GithubOAuth/Catch/?url=" + url;
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet]
        public async Task Catch(string url)
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));
                var user = await github.User.Current();
                HttpContext.Session.SetString("GitHub", user.Login);
            }
            url = url.Replace('*', '/');
            Response.Redirect(url);
        }
    }
}