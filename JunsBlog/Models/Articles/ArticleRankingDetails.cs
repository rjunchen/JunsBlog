using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleRankingDetails
    {
        public string ArticleId { get; set; }
        public bool DidILike { get; set; }
        public int LikesCount { get; set; }
        public bool DidIDislike { get; set; }
        public int DislikesCount { get; set; }
        public bool DidIFavor { get; set; }
    }
}
