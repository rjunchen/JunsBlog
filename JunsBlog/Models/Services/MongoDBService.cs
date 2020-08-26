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
            var query = from p in articles.AsQueryable()
                       where p.Id == articleId
                       join o in users.AsQueryable() on p.AuthorId equals o.Id into userJoined
                       select new ArticleDetails()
                       {
                           Abstract = p.Abstract,
                           Content = p.Content,
                           CoverImage = p.CoverImage,
                           Id = p.Id,
                           Title = p.Title,
                           UpdatedOn = p.UpdatedOn,
                           CreatedOn = p.CreatedOn,
                           Views = p.Views,
                           Author = userJoined.First()
                       };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SearchResponse> SearchArticlesAsyc(int page, int pageSize, string searchKey, string sortBy, SortOrderEnum sortOrder)
        {
            var filterDefinition = String.IsNullOrEmpty(searchKey)
                ? FilterDefinition<Article>.Empty : Builders<Article>.Filter.Where(x => x.Content.Contains(searchKey));

            var sortDefintion = sortOrder == SortOrderEnum.Ascending
                ? Builders<Article>.Sort.Ascending(sortBy)
                : Builders<Article>.Sort.Descending(sortBy);

            var totalDocuments = await articles.CountDocumentsAsync(filterDefinition);

            var documents = await articles.Find(filterDefinition).Skip((page - 1) * pageSize).Limit(pageSize).Sort(sortDefintion).ToListAsync();

            var articleDetailsList = new List<ArticleDetails>();

            foreach (Article document in documents)
            {
                var articleDetails = new ArticleDetails();

                articleDetails.Title = document.Title;
                articleDetails.Content = document.Content;
                articleDetails.Abstract = document.Abstract;
                articleDetails.CoverImage = document.CoverImage;
                articleDetails.Id = document.Id;
                articleDetails.UpdatedOn = document.UpdatedOn;
                articleDetails.Views = document.Views;
                articleDetails.Author = await FindUserAsync(x => x.Id == document.AuthorId);

                articleDetailsList.Add(articleDetails);
            }

            var searchResponse = new SearchResponse(articleDetailsList, (int)totalDocuments, page, pageSize, searchKey, sortBy, sortOrder);

            return searchResponse;
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
