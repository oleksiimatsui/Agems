
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
namespace Agems
{
    [Route("[controller]/[action]")]
    public class GithubOAuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}