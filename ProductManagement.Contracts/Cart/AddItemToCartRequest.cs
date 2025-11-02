using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Cart;

public record AddItemToCartRequest
{
    [Required]
    public Guid ProductId { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; init; }
}