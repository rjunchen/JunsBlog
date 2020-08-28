using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
using JunsBlog.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Test.Mockups
{
    public class DatabaseServiceFake : IDatabaseService
    {
        private readonly List<User> users;
        private readonly List<UserToken> userTokens;
        private readonly List<Article> articles;
        private readonly List<ArticleRanking> articleRankings;
        private readonly List<CommentRanking> commentRankings;
        public DatabaseServiceFake()
        {
            users = new List<User>();
            userTokens = new List<UserToken>();
            articles = new List<Article>();
            articleRankings = new List<ArticleRanking>();
            commentRankings = new List<CommentRanking>();
        }

        public async Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter)
        {
            var predic = new Predicate<Article>(filter.Compile());
            return await Task.Run(() => articles.Find(predic));
        }

        public async Task<ArticleRanking> FindArticleRankingAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            var predic = new Predicate<ArticleRanking>(filter.Compile());
            return await Task.Run(() => articleRankings.Find(predic));
        }

        public async Task<List<ArticleRanking>> FindArticleRankingsAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            return await Task.Run(() => articleRankings.Where(filter.Compile()).ToList());
        }

        public async Task<CommentRanking> FindCommentRankingAsync(Expression<Func<CommentRanking, bool>> filter)
        {
            var predic = new Predicate<CommentRanking>(filter.Compile());
            return await Task.Run(() => commentRankings.Find(predic));
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> filter)
        {
            var predic = new Predicate<User>(filter.Compile());
            return await Task.Run(() => users.Find(predic));
        }

 
        public async Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter)
        {
            var predic = new Predicate<UserToken>(filter.Compile());
            return await Task.Run(() => userTokens.Find(predic));
        }

        public async Task<ArticleDetails> GetArticleDetailsAsync(string articleId)
        {
            throw new NotImplementedException();
        }

        public async Task<ArticleRankingDetails> GetArticleRankingDetailsAsync(string articleId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentRankingDetails> GetCommentRankingDetails(string commentId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetCommentsAsync(string targetId)
        {
            throw new NotImplementedException();
        }

        public async Task<Article> SaveArticleAsync(Article article)
        {
            throw new NotImplementedException();
        }

        public async Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking)
        {
            throw new NotImplementedException();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await Task.Run(() => {
                return users.ReplaceOne(user);
            });
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            return await Task.Run(() => {
                return userTokens.ReplaceOne(userToken);
            });
        }

        public async Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentSearchPagingResult> SearchCommentsAsync(int page, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder, string currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
