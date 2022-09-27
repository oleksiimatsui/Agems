using System;
using System.Collections.Generic;

namespace Agems
{
    public partial class Category
    {
        public Category()
        {
            Sounds = new HashSet<Sound>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Sound> Sounds { get; set; }
    }
}
