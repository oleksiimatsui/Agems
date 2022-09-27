//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Octokit;
//using Octokit.Internal;


//namespace Agems.Models
//{
//    public partial class GitHubUser
//    {
//        public string Name { get; set; }
//        public string Pic { get; set; }
//        public async Task OnGetAsync()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                string accessToken = await ViewContext.HttpContext.Authentication("access_token");
//                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));

//                var user = await github.User.Current();

//                if (user.Name is null)
//                {
//                    Name = user.Login;
//                }
//                else
//                {
//                    Name = user.Name;
//                }
//                Pic = user.AvatarUrl;
//            }
//        }
//    }
//}
