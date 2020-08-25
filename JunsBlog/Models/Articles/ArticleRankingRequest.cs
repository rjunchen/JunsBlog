using JunsBlog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleRankingRequest
    {
        public string ArticleId { get; set; }
        public RankEnum Rank { get; set; }
    }
}
