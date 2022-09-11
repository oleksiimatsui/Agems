using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using Octokit.Internal;
namespace Agems
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; }
        public string Pic { get; set; }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));

                var user = await github.User.Current();

                if (user.Name is null)
                {
                    Name = user.Login;
                }
                else
                {
                    Name = user.Name;
                }
                Pic = user.AvatarUrl;

            }
        }
    }


}