using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Contracts.Orders;

public record CancelOrderRequest
{
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Reason { get; init; } = "Cancelled by user";
}