using System;
using System.Collections.Generic;

namespace Agems
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Sounds = new HashSet<Sound>();
        }

        public static object Identity { get; internal set; }
        public int Id { get; set; }
        public string GitHub { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Sound> Sounds { get; set; }


    }
}
