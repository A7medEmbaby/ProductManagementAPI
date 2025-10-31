using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Products;

public record AddStockRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; init; }
}