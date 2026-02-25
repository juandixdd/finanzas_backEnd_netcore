using BaseBackend.Domain.Entities;

namespace BaseBackend.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(int userId);
    Task<Category?> GetByIdAsync(int userId, int categoryId);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int categoryId);
}