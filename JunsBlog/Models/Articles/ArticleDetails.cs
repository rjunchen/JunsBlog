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
        public User Author { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Views { get; set; }
        public string Content { get; set; }
        public string[] Categories { get; set; }
        public string Id { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsApproved { get; set; }
        public int CommentsCount { get; set; }
        public ArticleRankingDetails Ranking { get; set; }
    }
}
