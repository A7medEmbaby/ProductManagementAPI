using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.Products.DTOs;

public record UpdateProductRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Required]
    public Guid CategoryId { get; init; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; init; }

    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; init; } = "USD";
}