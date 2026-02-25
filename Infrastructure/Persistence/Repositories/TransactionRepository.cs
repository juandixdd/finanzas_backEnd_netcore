using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseBackend.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────
    // GET ALL (solo del usuario)
    // ─────────────────────────────
    public async Task<IEnumerable<Transaction>> GetAllAsync(int userId)
    {
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    // ─────────────────────────────
    // GET BY ID (usuario + id)
    // ─────────────────────────────
    public async Task<Transaction?> GetByIdAsync(int userId, int transactionId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t =>
                t.Id == transactionId &&
                t.UserId == userId
            );
    }

    // ─────────────────────────────
    // GET BY FILTER
    // ─────────────────────────────
    public async Task<IEnumerable<Transaction>> GetByFilterAsync(
        int userId,
        DateTime? from,
        DateTime? to,
        int? categoryId,
        int? type
    )
    {
        var query = _context.Transactions
            .Where(t => t.UserId == userId)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        return await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    public async Task CreateAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────
    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────
    // DELETE (hard delete)
    // ─────────────────────────────
    public async Task DeleteAsync(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}