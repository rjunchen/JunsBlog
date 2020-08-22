using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JunsBlog.Test.Mockups
{
    public class DatabaseServiceFake : IDatabaseService
    {
        private readonly List<User> users;
        private readonly List<UserToken> userTokens;
        public DatabaseServiceFake()
        {
            users = new List<User>();
            userTokens = new List<UserToken>();
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
           return await Task.Run(() => users.Find(x => x.Email == email));
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            return await Task.Run(() => users.Find(x => x.Id == userId));
        }

        public async Task<UserToken> FindUserTokenByIdAsync(User user)
        {
            return await Task.Run(() => userTokens.Find(x => x.UserId == user.Id));
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
