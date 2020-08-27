using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Enums;
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
        private readonly IMongoCollection<CommentRanking> commentRanking;
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
            commentRanking = database.GetCollection<CommentRanking>(settings.CommentRankingCollectionName);
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

            var query = from a in articles.AsQueryable()
                       where a.Id == articleId
                       join u in users.AsQueryable() on a.AuthorId equals u.Id into userJoined
                       select new ArticleDetails()
                       {
                           Abstract = a.Abstract,
                           Content = a.Content,
                           CoverImage = a.CoverImage,
                           Id = a.Id,
                           Title = a.Title,
                           UpdatedOn = a.UpdatedOn,
                           CreatedOn = a.CreatedOn,
                           Views = a.Views,
                           IsApproved = a.IsApproved,
                           IsPrivate = a.IsPrivate,
                           Author = userJoined.First()
                       };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ArticleSearchPagingResult> SearchArticlesAsyc(int page, int pageSize, string searchKey, SortByEnum sortBy, SortOrderEnum sortOrder)
        {
            var query = articles.AsQueryable().GroupJoin(comments.AsQueryable(), x => x.Id, y => y.TargetId, (x, y) => new { article = x, comments = y })
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


            if (!string.IsNullOrEmpty(searchKey)) query = query.Where(x => x.Content.Contains(searchKey));

            switch (sortBy)
            {
                case SortByEnum.CreatedOn:
                    if (sortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.CreatedOn);
                    else
                        query = query.OrderByDescending(x => x.CreatedOn);
                    break;
                case SortByEnum.Views:
                    if (sortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.Views);
                    else
                        query = query.OrderByDescending(x => x.Views);
                    break;
            }

            var documents = await query.Skip(page - 1).Take(pageSize).ToListAsync();

            return new ArticleSearchPagingResult(documents, documents.Count, page, pageSize, searchKey, sortBy, sortOrder);
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
            return await comments.Find<Comment>(x=> x.TargetId == targetId).ToListAsync();
        }

        public async Task<List<CommentRanking>> FindCommentRankingsAsync(Expression<Func<CommentRanking, bool>> filter)
        {
            return await commentRanking.Find<CommentRanking>(filter).ToListAsync();
        }
    }
}
