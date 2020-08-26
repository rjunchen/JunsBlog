using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
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
        private readonly IMongoCollection<ArticleRanking> rankings;
        private readonly IMongoCollection<CommentRanking> commentRanking;
        private readonly IMongoCollection<Comment> comments;

        public MongoDBService(IJunsBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.UserTokensCollectionName);
            articles = database.GetCollection<Article>(settings.ArticleCollectionName);
            rankings = database.GetCollection<ArticleRanking>(settings.RankingCollectionName);
            comments = database.GetCollection<Comment>(settings.CommentCollectionName);
            commentRanking = database.GetCollection<CommentRanking>(settings.CommentRankingCollectionName);
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> filter)
        {
            return await users.Find<User>(filter).SingleOrDefaultAsync();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Id))
                user.Id = ObjectId.GenerateNewId().ToString();

            await users.ReplaceOneAsync(s => s.Id == user.Id, user, new ReplaceOptions { IsUpsert = true });
            return user;
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            if (String.IsNullOrWhiteSpace(userToken.Id))
                userToken.Id = ObjectId.GenerateNewId().ToString();

            await userTokens.ReplaceOneAsync(s => s.Id == userToken.Id, userToken, new ReplaceOptions { IsUpsert = true });
            return userToken;
        }

        public async Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter)
        {
            return await userTokens.Find<UserToken>(filter).FirstOrDefaultAsync();
        }

        public async Task<Article> SaveArticleAsync(Article article)
        {
            if (String.IsNullOrWhiteSpace(article.Id))
                article.Id = ObjectId.GenerateNewId().ToString();

            await articles.ReplaceOneAsync(s => s.Id == article.Id, article, new ReplaceOptions { IsUpsert = true });

            return article;
        }

        public async Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter)
        {
            var updateDef = Builders<Article>.Update.Inc(x => x.Views, 1);
            return await articles.FindOneAndUpdateAsync<Article>(filter, updateDef, 
                new FindOneAndUpdateOptions<Article, Article> { ReturnDocument = ReturnDocument.After });
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

        public async Task<List<ArticleRanking>> FindRankingsAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            return await rankings.Find<ArticleRanking>(filter).ToListAsync();
        }

        public async Task<ArticleRanking> FindRankingAsync(Expression<Func<ArticleRanking, bool>> filter)
        {
            return await rankings.Find<ArticleRanking>(filter).FirstOrDefaultAsync();
        }

        public async Task<ArticleRanking> SaveRankingAsync(ArticleRanking ranking)
        {
            if (String.IsNullOrWhiteSpace(ranking.Id))
                ranking.Id = ObjectId.GenerateNewId().ToString();

            await rankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            if (String.IsNullOrWhiteSpace(comment.Id))
            {
                comment.Id = ObjectId.GenerateNewId().ToString();
                comment.CreatedOn = DateTime.UtcNow;
            }

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
