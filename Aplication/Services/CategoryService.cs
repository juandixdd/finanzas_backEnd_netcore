using BaseBackend.Application.Common.Exceptions;
using BaseBackend.Domain.Dtos;
using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;

namespace BaseBackend.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // ─────────────────────────────
    // GET ALL
    // ─────────────────────────────
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(int userId)
    {
        var categories = await _categoryRepository.GetAllAsync(userId);

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type,
            Color = c.Color,
            Icon = c.Icon
        });
    }

    // ─────────────────────────────
    // GET BY ID
    // ─────────────────────────────
    public async Task<CategoryDto> GetByIdAsync(int userId, int id)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, id);

        if (category == null)
            throw new NotFoundException("Category not found");

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
            Color = category.Color,
            Icon = category.Icon
        };
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    public async Task CreateAsync(int userId, CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Category name is required");

        var category = new Category(
            userId,
            dto.Name,
            dto.Type,
            dto.Color,
            dto.Icon
        );

        await _categoryRepository.CreateAsync(category);
    }

    // ─────────────────────────────
    // UPDATE (CON TYPE)
    // ─────────────────────────────
    public async Task UpdateAsync(int userId, int id, UpdateCategoryDto dto)
    {
        Validate(dto);

        var category = await _categoryRepository.GetByIdAsync(userId, id);

        if (category == null)
            throw new NotFoundException("Category not found");

        category.Update(
            dto.Name,
            dto.Type,
            dto.Color,
            dto.Icon
        );

        await _categoryRepository.UpdateAsync(category);
    }

    // ─────────────────────────────
    // DELETE
    // ─────────────────────────────
    public async Task DeleteAsync(int userId, int id)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, id);

        if (category == null)
            throw new NotFoundException("Category not found");

        await _categoryRepository.DeleteAsync(id);
    }

    // ─────────────────────────────
    // VALIDATION
    // ─────────────────────────────
    private static void Validate(UpdateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Category name is required");

        if (string.IsNullOrWhiteSpace(dto.Color))
            throw new ArgumentException("Color is required");

        if (string.IsNullOrWhiteSpace(dto.Icon))
            throw new ArgumentException("Icon is required");
    }
}