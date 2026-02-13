using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using BaseBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseBackend.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id) // 👈 CAMBIADO
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) // 👈 CAMBIADO
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    
    public IQueryable<Product> Query()
    {
        return _context.Products.AsQueryable();
    }
}