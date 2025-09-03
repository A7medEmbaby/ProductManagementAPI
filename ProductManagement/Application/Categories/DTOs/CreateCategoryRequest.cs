using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.Categories.DTOs;

public record CreateCategoryRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;
}