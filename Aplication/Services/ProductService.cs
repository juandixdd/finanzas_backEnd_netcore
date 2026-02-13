using BaseBackend.Application.Common.Exceptions;
using BaseBackend.Application.Common.Pagination;
using BaseBackend.Application.DTOs;
using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseBackend.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // ✅ PAGINACIÓN PROFESIONAL
    public async Task<PagedResult<ProductDto>> GetAllAsync(PaginationParams pagination)
    {
        var query = _productRepository.Query();

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToListAsync();

        return new PagedResult<ProductDto>
        {
            Items = products,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize)
        };
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found");

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task CreateAsync(ProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required");

        if (dto.Price <= 0)
            throw new ValidationException("Price must be greater than zero");

        var product = new Product(dto.Name, dto.Price);

        await _productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(int id, ProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Product name is required");

        if (dto.Price <= 0)
            throw new ValidationException("Price must be greater than zero");

        product.Update(dto.Name, dto.Price);

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found");

        await _productRepository.DeleteAsync(id);
    }
}
