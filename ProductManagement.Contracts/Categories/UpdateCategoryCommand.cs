using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Categories;
public record UpdateCategoryCommand
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;
}