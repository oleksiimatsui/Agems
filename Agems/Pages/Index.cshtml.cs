using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using Octokit.Internal;
namespace Agems
{
    public class UserModel : PageModel
    {
        public string Pic { get; set; }
        public string Login { get; set; }
        public async Task OnGetAsync()
        {
            
        }
    }


}