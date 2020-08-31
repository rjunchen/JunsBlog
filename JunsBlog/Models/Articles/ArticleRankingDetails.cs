using JunsBlog.Entities;
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

        public ArticleRankingDetails(string articleId, string userId, IEnumerable<ArticleRanking> rankings)
        {
            this.ArticleId = articleId;
            foreach (var item in rankings)
            {
                if (item.DidIDislike) DislikesCount++;
                if (item.DidILike) LikesCount++;
                DidIFavor = item.DidIFavor;

                if (item.UserId == userId)
                {
                    DidIDislike = item.DidIDislike;
                    DidILike = item.DidILike;
                    DidIFavor = item.DidIFavor;
                }
            }
        }
    }
}
