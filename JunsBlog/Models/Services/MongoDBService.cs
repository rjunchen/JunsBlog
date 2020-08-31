using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
using MimeKit.Encodings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Models.Services
{
    public class MongoDBService : IDatabaseService
    {
        private readonly IMongoCollection<User> users;
        private readonly IMongoCollection<Article> articles;
        private readonly IMongoCollection<UserToken> userTokens;
        private readonly IMongoCollection<ArticleRanking> articleRankings;
        private readonly IMongoCollection<CommentRanking> commentRankings;
        private readonly IMongoCollection<Comment> comments;

        public MongoDBService(IJunsBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.UserTokensCollectionName);
            articles = database.GetCollection<Article>(settings.ArticleCollectionName);
            articleRankings = database.GetCollection<ArticleRanking>(settings.RankingCollectionName);
            comments = database.GetCollection<Comment>(settings.CommentCollectionName);
            commentRankings = database.GetCollection<CommentRanking>(settings.CommentRankingCollectionName);
        }

        #region Users
        public async Task<User> GetUserAsync(string userId)
        {
            var user = await users.Find<User>(x => x.Id == userId).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await users.Find<User>(x => x.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.UpdatedOn = DateTime.UtcNow;
            await users.ReplaceOneAsync(s => s.Id == user.Id, user, new ReplaceOptions { IsUpsert = true });
            return user;
        }
        #endregion


        #region UserTokens
        public async Task<UserToken> GetUserTokenAsync(string userId)
        {
            var userToken = await userTokens.Find<UserToken>(x => x.UserId == userId).SingleOrDefaultAsync();
            return userToken;
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            userToken.UpdatedOn = DateTime.UtcNow;
            await userTokens.ReplaceOneAsync(s => s.Id == userToken.Id, userToken, new ReplaceOptions { IsUpsert = true });
            return userToken;
        }
        #endregion


        #region Articles
        public async Task<Article> SaveArticleAsync(Article article)
        {
            article.UpdatedOn = DateTime.UtcNow;
            await articles.ReplaceOneAsync(s => s.Id == article.Id, article, new ReplaceOptions { IsUpsert = true });
            return article;
        }
        public async Task<Article> GetArticleAsync(string articleId)
        {
            return await articles.Find(s => s.Id == articleId).SingleOrDefaultAsync();
        }
        #endregion


        #region ArtileRankings
        public async Task<ArticleRanking> GetArticleRankingAsync(string articleId, string userId)
        {
            return await articleRankings.Find<ArticleRanking>(x => x.ArticleId == articleId && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<ArticleRanking>> GetArticleRankingsAsync(string articleId)
        {
            return await articleRankings.Find<ArticleRanking>(x => x.ArticleId == articleId).ToListAsync();
        }

        public async Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await articleRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }
        #endregion


        #region Comments
        public async Task<List<Comment>> GetCommentsAsync(string articleId)
        {
            return await comments.Find<Comment>(x => x.ArticleId == articleId).ToListAsync();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            comment.UpdatedOn = DateTime.UtcNow;

            await comments.ReplaceOneAsync(s => s.Id == comment.Id, comment, new ReplaceOptions { IsUpsert = true });

            return comment;
        }
        #endregion


        #region CommentRanking
        public async Task<CommentRanking> GetCommentRankingAsync(string commentId, string userId)
        {
            return await commentRankings.Find<CommentRanking>(x => x.CommentId == commentId && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<CommentRanking>> GetCommentRankingsAsync(string commentId)
        {
            return await commentRankings.Find<CommentRanking>(x => x.CommentId == commentId).ToListAsync();
        }

        public async Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await commentRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }
        #endregion


        #region Details
        public async Task<ArticleDetails> GetArticleDetailsAsync(string articleId)
        {
            // Increase the view count by 1
            var updateDef = Builders<Article>.Update.Inc(x => x.Views, 1);
            await articles.UpdateOneAsync<Article>(x => x.Id == articleId, updateDef);

            var query = GenerateArticleDetailsQuery();

            return await query.Where(x => x.Id == articleId).FirstOrDefaultAsync();
        }


        private class ArticleWithRankings : ArticleDetails
        {
           public IEnumerable<ArticleRanking> Rankings { get; set; }
        }

        private IMongoQueryable<ArticleWithRankings> GenerateArticleDetailsQuery()
        {
            var query = articles.AsQueryable().GroupJoin(comments.AsQueryable(), x => x.Id, y => y.ArticleId, (x, y) => new { article = x, comments = y })
                            .Select(a => new
                            {
                                Abstract = a.article.Abstract,
                                CoverImage = a.article.CoverImage,
                                Id = a.article.Id,
                                Title = a.article.Title,
                                UpdatedOn = a.article.UpdatedOn,
                                CreatedOn = a.article.CreatedOn,
                                IsApproved = a.article.IsApproved,
                                IsPrivate = a.article.IsPrivate,
                                Views = a.article.Views,
                                Content = a.article.Content,
                                AuthorId = a.article.AuthorId,
                                Categories =  a.article.Categories,
                                commentsCount = a.comments.Count()
                            }).Join(users.AsQueryable(), x => x.AuthorId, y => y.Id, (x, y) => new ArticleDetails
                            {
                                Abstract = x.Abstract,
                                CoverImage = x.CoverImage,
                                Id = x.Id,
                                Title = x.Title,
                                UpdatedOn = x.UpdatedOn,
                                CreatedOn = x.CreatedOn,
                                IsApproved = x.IsApproved,
                                IsPrivate = x.IsPrivate,
                                Views = x.Views,
                                Content = x.Content,
                                Categories = x.Categories,
                                Author = y,
                                CommentsCount = x.commentsCount
                            }).GroupJoin(articleRankings.AsQueryable(), x=> x.Id, y=> y.ArticleId, (x, y)=> new ArticleWithRankings {
                                Abstract = x.Abstract,
                                CoverImage = x.CoverImage,
                                Id = x.Id,
                                Title = x.Title,
                                UpdatedOn = x.UpdatedOn,
                                CreatedOn = x.CreatedOn,
                                IsApproved = x.IsApproved,
                                IsPrivate = x.IsPrivate,
                                Views = x.Views,
                                Content = x.Content,
                                Categories = x.Categories,
                                Author = x.Author,
                                CommentsCount = x.CommentsCount,
                                Rankings = y
                            });
            return query;
        }

        public async Task<ArticleSearchPagingResult> SearchArticlesAsyc(ArticleSearchPagingOption options, string currentUserId)
        {
            var query = GenerateArticleDetailsQuery();

            if (!string.IsNullOrEmpty(options.SearchKey)) query = query.Where(x => x.Content.ToLower().Contains(options.SearchKey.ToLower()));

            switch (options.SortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.UpdatedOn);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOn);
                    break;
                case SortByEnum.Views:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.Views);
                    else
                        query = query.OrderByDescending(x => x.Views);
                    break;
            }

            switch (options.Filter)
            {
                case ArticleFilterEnum.MyArticles:
                    query = query.Where(x => x.Author.Id == currentUserId);
                    break;
                case ArticleFilterEnum.MyLikes:
                    query = query.Where(x => x.Rankings.Any(x=> x.DidILike == true && x.UserId == currentUserId));
                    break;
                case ArticleFilterEnum.MyFavorites:
                    query = query.Where(x => x.Rankings.Any(x => x.DidIFavor == true && x.UserId == currentUserId));
                    break;
            }

            var docsCount = await query.CountAsync();

            var documents = await query.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync();

            var articleDetailsList = new List<ArticleDetails>();

            foreach (var item in documents)
            {
                item.Content = null; // Don't return the content
                var articleDetails = new ArticleDetails()
                {
                    Abstract = item.Abstract,
                    CoverImage = item.CoverImage,
                    Id = item.Id,
                    Title = item.Title,
                    UpdatedOn = item.UpdatedOn,
                    CreatedOn = item.CreatedOn,
                    IsApproved = item.IsApproved,
                    IsPrivate = item.IsPrivate,
                    Views = item.Views,
                    Content = item.Content,
                    Categories = item.Categories,
                    Author = item.Author,
                    CommentsCount = item.CommentsCount,
                    Ranking = new ArticleRankingDetails(item.Id, currentUserId, item.Rankings)
                };
                articleDetailsList.Add(articleDetails);
            }

            return new ArticleSearchPagingResult(articleDetailsList, docsCount, options);
        }


        public async Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId)
        {
            var rankingDetails = await GetCommentRankingDetailsAsync(commentId, currentUserId);

            var query = GenerateCommentsDetailsQuery();

            var commentDetail = await query.Where(x => x.Id == commentId).FirstOrDefaultAsync();
            commentDetail.Ranking = rankingDetails;
            return commentDetail;
        }

        private async Task<CommentRankingDetails> GetCommentRankingDetailsAsync(string commentId, string userId)
        {
            var rankings = await commentRankings.Find(x => x.CommentId == commentId).ToListAsync();

            var rankingResponse = new CommentRankingDetails() { CommentId = commentId };

            foreach (var item in rankings)
            {
                if (item.DidIDislike) rankingResponse.DislikesCount++;
                if (item.DidILike) rankingResponse.LikesCount++;
                rankingResponse.DidIFavor = item.DidIFavor;

                if (item.UserId == userId)
                {
                    rankingResponse.DidIDislike = item.DidIDislike;
                    rankingResponse.DidILike = item.DidILike;
                    rankingResponse.DidIFavor = item.DidIFavor;
                }
            }
            return rankingResponse;
        }

        private IMongoQueryable<CommentDetails> GenerateCommentsDetailsQuery()
        {
            var query = comments.AsQueryable().GroupJoin(comments.AsQueryable(), x => x.Id, y => y.ParentId, (x, y) => new { comment = x, childrenComments = y })
                            .Select(a => new
                            {
                                Id = a.comment.Id,
                                CommentText = a.comment.CommentText,
                                UpdatedOn = a.comment.UpdatedOn,
                                ArticleId = a.comment.ArticleId,
                                ChildrenCommentsCount = a.childrenComments.Count(),
                                ParentId = a.comment.ParentId,
                                UserId = a.comment.UserId,

                            }).Join(users.AsQueryable(), x => x.UserId, y => y.Id, (x, y) => new CommentDetails
                            {
                                Id = x.Id,
                                CommentText = x.CommentText,
                                UpdatedOn = x.UpdatedOn,
                                ArticleId = x.ArticleId,
                                ChildrenCommentsCount = x.ChildrenCommentsCount,
                                ParentId = x.ParentId,
                                User = y
                            });
            return query;
        } 

        public async Task<CommentSearchPagingResult> SearchCommentsAsync(CommentSearchPagingOption options, string currentUserId)
        {
            var query = GenerateCommentsDetailsQuery();

            if (!string.IsNullOrEmpty(options.SearchKey))
            {
                switch (options.SearchOn)
                {
                    case CommentSearchOnEnum.ArticleId:
                        query = query.Where(x => x.ArticleId == options.SearchKey.Trim());
                        break;
                    case CommentSearchOnEnum.ParentId:
                        query = query.Where(x => x.ParentId == options.SearchKey.Trim());
                        break;
                    case CommentSearchOnEnum.CommentText:
                        query = query.Where(x => x.CommentText.Contains(options.SearchKey.Trim()));
                        break;
                    default:
                        break;
                }
            }

            switch (options.SortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.UpdatedOn);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOn);
                    break;
                //case SortByEnum.Views:
                //    if (sortOrder == SortOrderEnum.Ascending)
                //        query = query.OrderBy(x => x.views);
                //    else
                //        query = query.OrderByDescending(x => x.Views);
                //    break;
            }

            var docsCount = await query.CountAsync();

            var documents = await query.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync();

            foreach (var item in documents)
            {
                item.Ranking = await GetCommentRankingDetailsAsync(item.Id, currentUserId);
            }

            return new CommentSearchPagingResult(documents, docsCount, options);
        }

        #endregion
    }
}
