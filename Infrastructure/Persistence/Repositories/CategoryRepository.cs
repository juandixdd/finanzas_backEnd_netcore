using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseBackend.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    
    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────
    // GET ALL (solo del usuario)
    // ─────────────────────────────
    public async Task<IEnumerable<Category>> GetAllAsync(int userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    // ─────────────────────────────
    // GET BY ID (usuario + id)
    // ─────────────────────────────
    public async Task<Category?> GetByIdAsync(int userId, int categoryId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    public async Task CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────
    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    // ─────────────────────────────
    // DELETE (usuario + id protegido desde Service)
    // ─────────────────────────────
    public async Task DeleteAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}