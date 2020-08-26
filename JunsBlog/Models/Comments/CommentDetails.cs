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
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; }
        public User Commenter { get; set; }
        public int CommentsCount { get; set; }
        public CommentRankingResponse Ranking { get; set; }
    }
}
