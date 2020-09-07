using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentRankingDetails
    {
        public bool DidILike { get; set; }
        public int LikesCount { get; set; }
        public bool DidIDislike { get; set; }
        public int DislikesCount { get; set; }
        public bool DidIFavor { get; set; }
    }
}
