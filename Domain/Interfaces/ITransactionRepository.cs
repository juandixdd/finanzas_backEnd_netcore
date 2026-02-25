using BaseBackend.Domain.Entities;

namespace BaseBackend.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(int userId);

    Task<Transaction?> GetByIdAsync(int userId, int transactionId);

    Task<IEnumerable<Transaction>> GetByFilterAsync(
        int userId,
        DateTime? from,
        DateTime? to,
        int? categoryId,
        int? type
    );

    Task CreateAsync(Transaction transaction);

    Task UpdateAsync(Transaction transaction);

    Task DeleteAsync(int transactionId);
}