using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
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
        Task<CommentRankingDetails> GetCommentRankingDetails(string commentId, string userId);
        Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder);
        Task<ArticleDetails> GetArticleDetailsAsync(string articleId);
        Task<CommentSearchPagingResult> SearchCommentsAsync(int page, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder, string currentUserId);
        Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId);
        
    }
}
