using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Test.Mockups
{
    public class DatabaseServiceFake : IDatabaseService
    {
        private readonly List<User> users;
        private readonly List<UserToken> userTokens;
        private readonly List<Article> articles;
        public DatabaseServiceFake()
        {
            users = new List<User>();
            userTokens = new List<UserToken>();
            articles = new List<Article>();
        }

        public async Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter)
        {
            var predic = new Predicate<Article>(filter.Compile());
            return await Task.Run(() => articles.Find(predic));
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

        public Task<Article> SaveArticleAsync(Article article)
        {
            throw new NotImplementedException();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await Task.Run(() => {
                var existingUser = users.Find(x => x.Email == user.Email);
                if (existingUser == null)
                {
                    user.Id = Guid.NewGuid().ToString(); // Generate a ID for the user record
                    users.Add(user);
                }
                else
                {
                    users.Remove(existingUser);
                    users.Add(user);
                };
                return user;
            });
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            return await Task.Run(() => {
                var existingUserToken = userTokens.Find(x => x.UserId == userToken.UserId);
                if (existingUserToken == null)
                {
                    userToken.Id = Guid.NewGuid().ToString(); // Generate a ID for the user record
                    userTokens.Add(userToken);
                }
                else
                {
                    userTokens.Remove(existingUserToken);
                    userTokens.Add(userToken);
                };
                return userToken;
            });
        }
    }
}
