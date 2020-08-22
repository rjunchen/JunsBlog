using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Services
{
    public class MongoDBService : IDatabaseService
    {
        private readonly IMongoCollection<User> users;
        private readonly IMongoCollection<UserToken> userTokens;

        public MongoDBService(IJunsBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.UserTokensCollectionName);
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await users.Find<User>(user => user.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();
        }

        public async Task<User> FindUserByIdAsync(string id)
        {
            return await users.Find<User>(user => user.Id == id).SingleOrDefaultAsync();
        }

        public async Task<User> SaveUserAsync(User user)
        {
           // Need to compare with emails, when comparing with Id the id is not generated on insert
           await users.ReplaceOneAsync(s => s.Email == user.Email, user, new ReplaceOptions { IsUpsert = true });
           return await FindUserByEmailAsync(user.Email);
        }

        public async Task<UserToken> SaveUserTokenAsync(UserToken userToken)
        {
            await userTokens.ReplaceOneAsync(s => s.UserId == userToken.UserId, userToken, new ReplaceOptions { IsUpsert = true });
            return await userTokens.Find<UserToken>(s => s.UserId == userToken.UserId).FirstOrDefaultAsync();
        }

        public async Task<UserToken> FindUserTokenByIdAsync(User user)
        {
            return await userTokens.Find<UserToken>(s => s.UserId == user.Id).FirstOrDefaultAsync();
        }
    }
}
