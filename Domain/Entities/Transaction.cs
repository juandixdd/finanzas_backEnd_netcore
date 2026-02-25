namespace BaseBackend.Domain.Entities;

public class Transaction
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public decimal Amount { get; private set; }
    public int Type { get; private set; } // 1 = Income, 2 = Expense
    public int CategoryId { get; private set; }
    public string? Description { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Transaction() { }

    public Transaction(
        int userId,
        decimal amount,
        int type,
        int categoryId,
        DateTime date,
        string? description = null
    )
    {
        UserId = userId;
        Amount = amount;
        Type = type;
        CategoryId = categoryId;
        Date = date;
        Description = description;

        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        decimal amount,
        int type,
        int categoryId,
        DateTime date,
        string? description
    )
    {
        Amount = amount;
        Type = type;
        CategoryId = categoryId;
        Date = date;
        Description = description;

        UpdatedAt = DateTime.UtcNow;
    }
}