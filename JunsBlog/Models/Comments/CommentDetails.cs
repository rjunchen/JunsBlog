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
        public User Commenter { get; set; }
        public int CommentsCount { get; set; }
        public CommentType CommentType { get; set; }
        public string TargetId { get; set; }
        public CommentRankingDetails Ranking { get; set; }

    }
}
