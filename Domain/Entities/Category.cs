namespace BaseBackend.Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public int UserId { get; private set; }

    public string Name { get; private set; } = null!;
    public int Type { get; private set; }   // 1 = Income, 2 = Expense
    public string Color { get; private set; } = null!;
    public string Icon { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Category() { }

    public Category(
        int userId,
        string name,
        int type,
        string color,
        string icon
    )
    {
        UserId = userId;
        Name = name;
        Type = type;
        Color = color;
        Icon = icon;

        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, int type, string color, string icon)
    {
        Name = name;
        Type = type;
        Color = color;
        Icon = icon;
        UpdatedAt = DateTime.UtcNow;
    }
}