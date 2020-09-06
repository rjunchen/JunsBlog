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

        public static ArticleRankingDetails GenerateArticleRankingDetails(List<string> likes, List<string> dislikes, List<string> favors, string currentUserId)
        {
            return new ArticleRankingDetails()
            {
                DidIFavor = favors.Contains(currentUserId),
                DidIDislike = dislikes.Contains(currentUserId),
                DidILike = likes.Contains(currentUserId),
                LikesCount = likes.Count,
                DislikesCount = dislikes.Count
            };  
        }

    }
}
