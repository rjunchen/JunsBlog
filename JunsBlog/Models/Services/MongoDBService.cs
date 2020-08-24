using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Articles;
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

        public MongoDBService(IJunsBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.UserTokensCollectionName);
            articles = database.GetCollection<Article>(settings.ArticleCollectionName);
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> filter)
        {
            return await users.Find<User>(filter).SingleOrDefaultAsync();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            // Need to compare with emails, when comparing with Id the id is not generated on insert
            await users.ReplaceOneAsync(s => s.Email == user.Email, user, new ReplaceOptions { IsUpsert = true });
            return user;
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            // Need to compare with UserId, when comparing with Id the id is not generated on insert
            await userTokens.ReplaceOneAsync(s => s.UserId == userToken.UserId, userToken, new ReplaceOptions { IsUpsert = true });
            return userToken;
        }

        public async Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter)
        {
            return await userTokens.Find<UserToken>(filter).FirstOrDefaultAsync();
        }

        public async Task<Article> SaveArticleAsync(Article article)
        {
           var existingArticle = await FindArticAsync(x => x.Id == article.Id);
            if (existingArticle == null)
                await articles.InsertOneAsync(article); 
            else
                await articles.ReplaceOneAsync(s => s.Id == article.Id, article, new ReplaceOptions { IsUpsert = true });

            return article;
        }

        public async Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter)
        {
            return await articles.Find<Article>(filter).FirstOrDefaultAsync();
        }

        public async Task<SearchResponse> SearchArticlesAsyc(int page, int pageSize, string searchKey, string sortOrder, string sortBy)
        {
            var filterDefinition = String.IsNullOrEmpty(searchKey)
                ? FilterDefinition<Article>.Empty : Builders<Article>.Filter.Where(x => true);

            Article article;

            var sortDefintion = Builders<Article>.Sort.Ascending(nameof(article.CreationDate));

            var totalDocuments = await articles.CountDocumentsAsync(filterDefinition);

            var documents = await articles.Find(x => true).Skip((page - 1) * pageSize).Limit(pageSize).Sort(sortDefintion).ToListAsync();

            var articleDetailsList = new List<ArticleDetails>();

            foreach (Article document in documents)
            {
                var articleDetails = new ArticleDetails();

                articleDetails.Title = document.Title;
                articleDetails.Content = document.Content;
                articleDetails.Abstract = document.Abstract;
                articleDetails.CoverImage = document.CoverImage;
                articleDetails.Id = document.Id;
                articleDetails.LastModifiedDate = document.CreationDate;
                articleDetails.Author = await FindUserAsync(x => x.Id == document.AuthorId);

                articleDetailsList.Add(articleDetails);
            }

            var searchResponse = new SearchResponse(articleDetailsList, (int)totalDocuments, page, pageSize, searchKey, sortOrder, sortBy);

            return searchResponse;
        }
    }
}
