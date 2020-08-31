using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
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


        // UserTokens
        Task<UserToken> GetUserTokenAsync(string userId);
        Task<UserToken> SaveUserTokenAsync(UserToken userToken);


        // Articles
        Task<Article> SaveArticleAsync(Article article);
        Task<Article> GetArticleAsync(string articleId);

        // ArticleRanking
        Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking);
        Task<List<ArticleRanking>> GetArticleRankingsAsync(string articleId);
        Task<ArticleRanking> GetArticleRankingAsync(string articleId, string userId);


        // Comments
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsAsync(string articleId);


        // CommentRankings
        Task<CommentRanking> GetCommentRankingAsync(string commentId, string userId);
        Task<List<CommentRanking>> GetCommentRankingsAsync(string commentId);
        Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking);


        // Details
        Task<ArticleSearchPagingResult> SearchArticlesAsyc(ArticleSearchPagingOption options, string currentUserId);
        Task<ArticleDetails> GetArticleDetailsAsync(string articleId);
        Task<CommentSearchPagingResult> SearchCommentsAsync(CommentSearchPagingOption options, string currentUserId);
        Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId);
        
    }
}
