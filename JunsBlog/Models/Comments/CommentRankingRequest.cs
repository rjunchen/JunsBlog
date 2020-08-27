using JunsBlog.Models.Enums;

namespace JunsBlog.Models.Comments
{
    public class CommentRankingRequest
    {
        public string CommentId { get; set; }
        public RankEnum Rank { get; set; }
    }
}
