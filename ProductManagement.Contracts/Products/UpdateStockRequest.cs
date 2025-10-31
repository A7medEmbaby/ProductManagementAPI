using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Products;

public record UpdateStockRequest
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int NewQuantity { get; init; }
}