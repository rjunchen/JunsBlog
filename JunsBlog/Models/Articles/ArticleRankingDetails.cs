using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleRankingDetails
    {
        public bool DidILike { get; set; }
        public int LikesCount { get; set; }
        public bool DidIDislike { get; set; }
        public int DislikesCount { get; set; }
        public bool DidIFavor { get; set; }

        public ArticleRankingDetails(ArticleRanking ranking, string currentUserId)
        {

            DidIFavor = ranking.Favors.Contains(currentUserId);
            DidIDislike = ranking.Dislikes.Contains(currentUserId);
            DidILike = ranking.Likes.Contains(currentUserId);
            LikesCount = ranking.Likes.Count;
            DislikesCount = ranking.Dislikes.Count;
        }

    }
}
