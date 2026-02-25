namespace BaseBackend.Domain.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Type { get; set; }
    public string Color { get; set; } = null!;
    public string Icon { get; set; } = null!;
}