using JunsBlog.Entities;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByIdAsync(string userId);
        Task<User> SaveUserAsync(User user);
        Task<UserToken> FindUserTokenByIdAsync(User user);
        Task<UserToken> SaveUserTokenAsync(UserToken userToken);
    }
}
