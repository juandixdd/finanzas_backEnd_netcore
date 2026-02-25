namespace BaseBackend.Domain.Dtos;

public class UpdateCategoryDto
{
    public string Name { get; set; } = null!;
    public int Type { get; set; }
    public string Color { get; set; } = null!;
    public string Icon { get; set; } = null!;
}