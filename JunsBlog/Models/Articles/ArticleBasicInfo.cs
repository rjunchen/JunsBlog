using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleBasicInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CoverImage { get; set; }  
        public string Abstract { get; set; }
        public bool IsPrivate { get; set; }
        public string[] Categories { get; set; }
    }
}
