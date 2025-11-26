using Banca_movil.Models;


namespace Banca_movil.Repositories
{
    public interface IAccountRepository
    {
        Task<List<AccountRepository>> GetAllAsync();
        Task<AccountRepository> GetByIdAsync(int id);
        Task AddAsync(AccountRepository account);
        Task UpdateAsync(AccountRepository account);
        Task DeleteAsync(int id);


    }
}
