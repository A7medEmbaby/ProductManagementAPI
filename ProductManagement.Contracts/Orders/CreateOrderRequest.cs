using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Orders;

public record CreateOrderRequest
{
    [Required]
    public Guid UserId { get; init; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must have at least one item")]
    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public record CreateOrderItemRequest
{
    [Required]
    public Guid ProductId { get; init; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string ProductName { get; init; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; init; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal UnitPrice { get; init; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; init; } = "USD";
}