using Banca_movil.Models;

namespace Banca_movil.Repo
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUserName(string userName);
        Task<User?> GetUserByEmail(string email);
        Task<User> AddAsync(User user);
        bool ValidatePassWord(User user, string passWord);
        Task SaveAsync();
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
