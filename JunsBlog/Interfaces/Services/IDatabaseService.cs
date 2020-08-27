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
        Task<User> FindUserAsync(Expression<Func<User, bool>> filter);
        Task<User> SaveUserAsync(User user);
        Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter);
        Task<UserToken> SaveUserTokenAsync(UserToken userToken);
        Task<Article> SaveArticleAsync(Article article);
        Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter);
        Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder);
        Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking);
        Task<List<ArticleRanking>> FindArticleRankingsAsync(Expression<Func<ArticleRanking, bool>> filter);
        Task<ArticleRanking> FindArticleRankingAsync(Expression<Func<ArticleRanking, bool>> filter);
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsAsync(string targetId);
        Task<List<CommentRanking>> FindCommentRankingsAsync(Expression<Func<CommentRanking, bool>> filter);
        Task<ArticleDetails> GetArticleDetailsAsync(string articleId);
        Task<ArticleRankingDetails> GetArticleRankingDetailsAsync(string articleId, string userId);
        Task<CommentSearchPagingResult> SearchCommentsAsync(int page, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder, string currentUserId);
        Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId);
    }
}
