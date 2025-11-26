using Banca_movil.Data;
using Banca_movil.Models;
using Banca_movil.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository(AppDbContext _context) : ITransactionRepository
{
    private readonly AppDbContext _context;
    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Transaction>> GetAllAsync() =>
        await _context.Transactions
                      .Include(t => t.Account)
                      .ThenInclude(a => a.User)
                      .ToListAsync();

    public async Task<Transaction> GetByIdAsync(int id) =>
        await _context.Transactions
                      .Include(t => t.Account)
                      .ThenInclude(a => a.User)
                      .FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }

    private Task<List<Transaction>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<Transaction> ITransactionRepository.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
