using BaseBackend.Application.Common.Exceptions;
using BaseBackend.Application.DTOs.Transaction;
using BaseBackend.Domain.Dtos;
using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;

namespace BaseBackend.Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository
    )
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    // ─────────────────────────────
    // GET ALL
    // ─────────────────────────────
    public async Task<IEnumerable<TransactionDto>> GetAllAsync(int userId)
    {
        var transactions = await _transactionRepository.GetAllAsync(userId);

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type,
            CategoryId = t.CategoryId,
            Description = t.Description,
            Date = t.Date
        });
    }

    // ─────────────────────────────
    // GET BY ID
    // ─────────────────────────────
    public async Task<TransactionDto> GetByIdAsync(int userId, int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(userId, id);

        if (transaction == null)
            throw new NotFoundException("Transaction not found");

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            Description = transaction.Description,
            Date = transaction.Date
        };
    }

    // ─────────────────────────────
    // GET BY FILTER
    // ─────────────────────────────
    public async Task<IEnumerable<TransactionDto>> GetByFilterAsync(
        int userId,
        DateTime? from,
        DateTime? to,
        int? categoryId,
        int? type
    )
    {
        var transactions = await _transactionRepository.GetByFilterAsync(
            userId,
            from,
            to,
            categoryId,
            type
        );

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type,
            CategoryId = t.CategoryId,
            Description = t.Description,
            Date = t.Date
        });
    }

    // ─────────────────────────────
    // CREATE
    // ─────────────────────────────
    public async Task CreateAsync(int userId, CreateTransactionDto dto)
    {
        Validate(dto);

        var category = await _categoryRepository.GetByIdAsync(userId, dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found");

        if (category.Type != dto.Type)
            throw new ArgumentException("Transaction type does not match category type");

        var transaction = new Transaction(
            userId,
            dto.Amount,
            dto.Type,
            dto.CategoryId,
            dto.Date,
            dto.Description
        );

        await _transactionRepository.CreateAsync(transaction);
    }

    // ─────────────────────────────
    // UPDATE
    // ─────────────────────────────
    public async Task UpdateAsync(int userId, int id, UpdateTransactionDto dto)
    {
        Validate(dto);

        var transaction = await _transactionRepository.GetByIdAsync(userId, id);

        if (transaction == null)
            throw new NotFoundException("Transaction not found");

        var category = await _categoryRepository.GetByIdAsync(userId, dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found");

        if (category.Type != dto.Type)
            throw new ArgumentException("Transaction type does not match category type");

        transaction.Update(
            dto.Amount,
            dto.Type,
            dto.CategoryId,
            dto.Date,
            dto.Description
        );

        await _transactionRepository.UpdateAsync(transaction);
    }

    // ─────────────────────────────
    // DELETE
    // ─────────────────────────────
    public async Task DeleteAsync(int userId, int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(userId, id);

        if (transaction == null)
            throw new NotFoundException("Transaction not found");

        await _transactionRepository.DeleteAsync(id);
    }

    // ─────────────────────────────
    // VALIDATION
    // ─────────────────────────────
    private static void Validate(CreateTransactionDto dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (dto.Type is not (1 or 2))
            throw new ArgumentException("Invalid transaction type");

        if (dto.Date == default)
            throw new ArgumentException("Date is required");
    }

    private static void Validate(UpdateTransactionDto dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (dto.Type is not (1 or 2))
            throw new ArgumentException("Invalid transaction type");

        if (dto.Date == default)
            throw new ArgumentException("Date is required");
    }
}