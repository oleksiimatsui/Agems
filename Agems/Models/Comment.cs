using System;
using System.Collections.Generic;

namespace Agems
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public int SoundId { get; set; }
        public string Text { get; set; } = null!;

        public virtual Sound Sound { get; set; } = null!;
    }
}
