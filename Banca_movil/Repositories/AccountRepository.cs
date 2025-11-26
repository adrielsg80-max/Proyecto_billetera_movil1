using Banca_movil.Data;
using Banca_movil.Models;
using Banca_movil.Repo;
using Microsoft.EntityFrameworkCore;

namespace Banca_movil.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync() =>
            await _context.Accounts.Include(a => a.User).ToListAsync();

        public async Task<Account> GetByIdAsync(int id) =>
            await _context.Accounts.Include(a => a.User)
                                   .FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        Task<List<AccountRepository>> IAccountRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<AccountRepository> IAccountRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(AccountRepository account)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AccountRepository account)
        {
            throw new NotImplementedException();
        }
    }
}