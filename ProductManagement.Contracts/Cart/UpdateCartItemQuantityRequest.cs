using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Cart;

public record UpdateCartItemQuantityRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int NewQuantity { get; init; }
}