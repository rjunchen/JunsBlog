using JunsBlog.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task<User> FindUserAsync(Expression<Func<User, bool>> filter);
        Task<User> SaveUserAsync(User user);
        Task<UserToken> FindUserTokenAsync(Expression<Func<UserToken, bool>> filter);
        Task<UserToken> SaveUserTokenAsync(UserToken userToken);
        Task<Article> SaveArticleAsync(Article article);
        Task<Article> FindArticAsync(Expression<Func<Article, bool>> filter);
    }
}
