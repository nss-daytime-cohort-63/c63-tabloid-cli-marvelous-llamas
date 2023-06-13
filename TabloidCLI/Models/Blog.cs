using System.Collections.Generic;

namespace TabloidCLI.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public override string ToString()
        {
            return $"{Title} ({URL})";
        }
    }
}