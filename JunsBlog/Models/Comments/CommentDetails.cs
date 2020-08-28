using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentDetails
    {
        public string Id { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string CommentText { get; set; }
        public User User { get; set; }
        public int ChildrenCommentsCount { get; set; }
        public string ArticleId { get; set; }
        public string ParentId { get; set; }
        public CommentRankingDetails Ranking { get; set; }

    }
}
