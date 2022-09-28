namespace Agems.Models
{
    public class GitHubUser
    {
        public string Name { get; set; }
        public string Pic { get; set; }
        public string Login { get; set; }

        public GitHubUser (string name, string pic, string login)
        {
            Login = login; Name = name; Pic = pic;
        }
    }
}
