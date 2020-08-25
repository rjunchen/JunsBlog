using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleRankingResponse
    {
        public string ArticleId { get; set; }
        public bool DidILike { get; set; }
        public int Likes { get; set; }
        public bool DidIDislike { get; set; }
        public int Dislikes { get; set; }
        public bool DidIFavor { get; set; }
    }
}
