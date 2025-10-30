using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Categories;
public record UpdateCategoryRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;
}