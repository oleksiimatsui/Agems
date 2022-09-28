namespace Agems.Models
{
    public static class GitHubUser
    {
        public static string Name { get; set; }
        public static string Pic { get; set; }

        public static string Login { get; set; }

        public static void SetUser(string name, string pic, string login)
        {
            Name = name;
            Pic = pic;
            Login = login;
        }
    }
}
