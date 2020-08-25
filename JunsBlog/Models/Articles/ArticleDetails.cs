using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleDetails
    {
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public string Abstract { get; set; }
        public ArticleRankingResponse Ranking { get; set; }
        public User Author { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Views { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }
    }
}
