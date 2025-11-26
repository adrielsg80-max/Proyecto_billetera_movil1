using Banca_movil.Data;
using Banca_movil.Models;
using Banca_movil.Repo;
using Microsoft.EntityFrameworkCore;

namespace Banca_movil.Repositories
{
    public class UserRepository(AppDbContext _context) : IUserRepository
    {
        public async Task<User> AddAsync(User user)
        {
            var entry = await _context.Users.AddAsync(user);
            return entry.Entity;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public bool ValidatePassWord(User user, string passaWord)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(passaWord, user.Password);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

    }
}