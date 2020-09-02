using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class GalleryImage
    {
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }

        public GalleryImage(string small, string medium, string big)
        {
            this.Small = small;
            this.Medium = medium;
            this.Big = big;
        }
    }
}
