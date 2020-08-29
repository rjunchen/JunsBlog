﻿using JunsBlog.Entities;
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
        private readonly List<Comment> comments;
        public DatabaseServiceFake()
        {
            users = new List<User>();
            userTokens = new List<UserToken>();
            articles = new List<Article>();
            articleRankings = new List<ArticleRanking>();
            commentRankings = new List<CommentRanking>();
            comments = new List<Comment>();
        }

        #region Users
        public async Task<User> GetUserAsync(string userId)
        {
            return await Task.Run(() => users.Find(x => x.Id == userId));
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await Task.Run(() => users.Find(x => x.Email.ToLower() == email.ToLower()));
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return users.ReplaceOne(user);
            });
        }
        #endregion


        #region UserTokens
        public async Task<UserToken> GetUserTokenAsync(string userId)
        {
            return await Task.Run(() => userTokens.Find(x => x.UserId == userId));
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            userToken.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return userTokens.ReplaceOne(userToken);
            });
        }
        #endregion


        #region Article
        public async Task<Article> SaveArticleAsync(Article article)
        {
            article.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return articles.ReplaceOne(article);
            });
        }
        #endregion


        #region ArticleRankings
        public async Task<ArticleRanking> GetArticleRankingAsync(string articleId, string userId)
        {
            return await Task.Run(() => {
                return articleRankings.Find(x => x.ArticleId == articleId && x.UserId == userId);
            });
        }

        public async Task<List<ArticleRanking>> GetArticleRankingsAsync(string articleId)
        {
            return await Task.Run(() => {
                return articleRankings.Where(x => x.ArticleId == articleId).ToList();
            });
        }

        public async Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return articleRankings.ReplaceOne(ranking);
            });
        }
        #endregion


        #region Comment
        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            comment.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return comments.ReplaceOne(comment);
            });
        }

        public async Task<List<Comment>> GetCommentsAsync(string articleId)
        {
            return await Task.Run(() => {
                return comments.Where(x => x.ArticleId == articleId).ToList();
            });
        }
        #endregion


        #region CommentRankings
        public async Task<CommentRanking> GetCommentRankingAsync(string commentId, string userId)
        {
            return await Task.Run(() => {
                return commentRankings.Find(x => x.CommentId == commentId && x.UserId == userId);
            });
        }

        public async Task<List<CommentRanking>> GetCommentRankingsAsync(string commentId)
        {
            return await Task.Run(() => {
                return commentRankings.Where(x => x.CommentId == commentId).ToList();
            });
        }

        public async Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            return await Task.Run(() => {
                return commentRankings.ReplaceOne(ranking);
            });
        }
        #endregion



        public Task<CommentRankingDetails> GetCommentRankingDetails(string commentId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder)
        {
            throw new NotImplementedException();
        }

        public async Task<ArticleDetails> GetArticleDetailsAsync(string articleId)
        {
            return await Task.Run(async () => {
                var article = articles.FirstOrDefault(x => x.Id == articleId);
                if (article == null) return null;
                article.Views++;

                var articleDetails = new ArticleDetails()
                {
                    Title = article.Title,
                    Abstract = article.Abstract,
                    Content = article.Content,
                    CoverImage = article.CoverImage,
                    CreatedOn = article.CreatedOn,
                    UpdatedOn = article.UpdatedOn,
                    Id = article.Id,
                    Views = article.Views,
                    IsApproved = article.IsApproved,
                    IsPrivate = article.IsPrivate,
                    Author = await GetUserAsync(article.AuthorId),
                    CommentsCount = GetCommentsAsync(articleId).Result.Count
                };

                return articleDetails;
            });
        }

        public Task<CommentSearchPagingResult> SearchCommentsAsync(int page, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder, string currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId)
        {
            throw new NotImplementedException();
        }
   



    }
}
