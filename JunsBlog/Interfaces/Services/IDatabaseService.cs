using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
using JunsBlog.Models.Profile;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Services
{
    public interface IDatabaseService
    {
        // Users
        Task<User> SaveUserAsync(User user);
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<ProfileDetails> GetProfileDetailsAsync(string currentUserId);

        // Articles
        Task<Article> SaveArticleAsync(Article article);
        Task<Article> GetArticleAsync(string articleId);
        Task<ArticleBasicInfo> GetArticleBasicInfoAsync(string articleId);
        Task<ArticleDetails> GetArticleDetailsAsync(string articleId, string currentUserId);
        Task<ArticleSearchPagingResult> SearchArticlesAsyc(ArticleSearchPagingOption options);

        // ArticleRanking
        Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking);
        Task<List<ArticleRanking>> GetArticleRankingsAsync(string articleId);
        Task<ArticleRanking> GetArticleRankingAsync(string articleId, string userId);

        // Comments
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<CommentSearchPagingResult> SearchCommentsAsync(CommentSearchPagingOption options, string currentUserId);
        //  Task<List<Comment>> GetCommentsAsync(string articleId);


        // CommentRankings
        Task<CommentRanking> GetCommentRankingAsync(string commentId, string userId);
        Task<List<CommentRanking>> GetCommentRankingsAsync(string commentId);
        Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking);
        Task<CommentRankingDetails> GetCommentRankingDetailsAsync(string commentId, string userId);
    }
}
