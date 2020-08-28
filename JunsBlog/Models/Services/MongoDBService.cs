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

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> filter)
        {
            return await users.Find<User>(filter).SingleOrDefaultAsync();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.UpdatedOn = DateTime.UtcNow;
            await users.ReplaceOneAsync(s => s.Id == user.Id, user, new ReplaceOptions { IsUpsert = true });
            return user;
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            userToken.UpdatedOn = DateTime.UtcNow;
            await userTokens.ReplaceOneAsync(s => s.Id == userToken.Id, userToken, new ReplaceOptions { IsUpsert = true });
            return userToken;
        }

        public async Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter)
        {
            return await userTokens.Find<UserToken>(filter).FirstOrDefaultAsync();
        }

        public async Task<Article> SaveArticleAsync(Article article)
        {
            article.UpdatedOn = DateTime.UtcNow;
            await articles.ReplaceOneAsync(s => s.Id == article.Id, article, new ReplaceOptions { IsUpsert = true });
            return article;
        }

        public async Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter)
        {
            var updateDef = Builders<Article>.Update.Inc(x => x.Views, 1);
            return await articles.FindOneAndUpdateAsync<Article>(filter, updateDef, 
                new FindOneAndUpdateOptions<Article, Article> { ReturnDocument = ReturnDocument.After });
        }


        public async Task<ArticleDetails> GetArticleDetailsAsync(string articleId)
        {
            // Increase the view count by 1
            var updateDef = Builders<Article>.Update.Inc(x => x.Views, 1);
            await articles.UpdateOneAsync<Article>(x => x.Id == articleId, updateDef);

            var query = GenerateArticleDetailsQuery();

            return await query.Where(x => x.Id == articleId).FirstOrDefaultAsync();
        }

        private IMongoQueryable<ArticleDetails> GenerateArticleDetailsQuery()
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
                                AuthorId = a.article.AuthorId,
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
                                Author = y,
                                CommentsCount = x.commentsCount
                            });
            return query;
        }

        public async Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder)
        {
            var query = GenerateArticleDetailsQuery();

            if (!string.IsNullOrEmpty(searchKey)) query = query.Where(x => x.Content.Contains(searchKey));

            switch (sortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (sortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.UpdatedOn);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOn);
                    break;
                case SortByEnum.Views:
                    if (sortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.Views);
                    else
                        query = query.OrderByDescending(x => x.Views);
                    break;
            }

            var docsCount = await query.CountAsync();

            var documents = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new ArticleSearchPagingResult(documents, docsCount, page, pageSize, searchKey, sortBy, sortOrder);
        }

        public async Task<List<ArticleRanking>> FindArticleRankingsAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            return await articleRankings.Find<ArticleRanking>(filter).ToListAsync();
        }

        public async Task<ArticleRanking> FindArticleRankingAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            return await articleRankings.Find<ArticleRanking>(filter).FirstOrDefaultAsync();
        }

        public async Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await articleRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }

        public async Task<ArticleRankingDetails> GetArticleRankingDetailsAsync(string articleId, string userId)
        {
            var rankings = await articleRankings.Find(x => x.ArticleId == articleId).ToListAsync();

            var rankingResponse = new ArticleRankingDetails() { ArticleId = articleId };

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

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            comment.UpdatedOn = DateTime.UtcNow;

            await comments.ReplaceOneAsync(s => s.Id == comment.Id, comment, new ReplaceOptions { IsUpsert = true });

            return comment;
        }

        public async Task<List<Comment>> GetCommentsAsync(string targetId)
        {
            return await comments.Find<Comment>(x=> x.ArticleId == targetId).ToListAsync();
        }

        public async Task<CommentRanking> FindCommentRankingAsync(Expression<Func<CommentRanking, bool>> filter)
        {
            return await commentRankings.Find<CommentRanking>(filter).FirstOrDefaultAsync();
        }

        public async Task<CommentDetails> GetCommentDetialsAsync(string commentId, string currentUserId)
        {
            var rankingDetails = await GetCommentRankingDetails(commentId, currentUserId);

            var query = GenerateCommentsDetailsQuery();

            var commentDetail = await query.Where(x => x.Id == commentId).FirstOrDefaultAsync();
            commentDetail.Ranking = rankingDetails;
            return commentDetail;
        }

        public async Task<CommentRankingDetails> GetCommentRankingDetails(string commentId, string userId)
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

        public async Task<CommentSearchPagingResult> SearchCommentsAsync(int page, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder, string currentUserId = null)
        {
            var query = GenerateCommentsDetailsQuery();

            if (!string.IsNullOrEmpty(searchKey))
            {
                switch (searchOn)
                {
                    case CommentSearchOnEnum.ArticleId:
                        query = query.Where(x => x.ArticleId == searchKey.Trim());
                        break;
                    case CommentSearchOnEnum.ParentId:
                        query = query.Where(x => x.ParentId == searchKey.Trim());
                        break;
                    case CommentSearchOnEnum.CommentText:
                        query = query.Where(x => x.CommentText.Contains(searchKey.Trim()));
                        break;
                    default:
                        break;
                }
            }

            switch (sortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (sortOrder == SortOrderEnum.Ascending)
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

            var documents = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            foreach (var item in documents)
            {
                item.Ranking = await GetCommentRankingDetails(item.Id, currentUserId);
            }

            return new CommentSearchPagingResult(documents, docsCount, page, pageSize, searchKey, searchOn, sortBy, sortOrder);
        }

        public async Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await commentRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }
    }
}
