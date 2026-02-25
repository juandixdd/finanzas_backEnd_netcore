namespace BaseBackend.Domain.Dtos;

public class CreateCategoryDto
{
    public string Name { get; set; } = null!;
    public int Type { get; set; }   // 1 = Income, 2 = Expense
    public string Color { get; set; } = null!;
    public string Icon { get; set; } = null!;
}