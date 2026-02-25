namespace BaseBackend.Application.DTOs.Transaction;

public class UpdateTransactionDto
{
    public decimal Amount { get; set; }
    public int Type { get; set; } // 1 = Income, 2 = Expense
    public int CategoryId { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}