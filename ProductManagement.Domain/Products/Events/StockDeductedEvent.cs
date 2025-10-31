using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record StockDeductedEvent(
    ProductId ProductId,
    int Quantity,
    int RemainingQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static StockDeductedEvent Create(ProductId productId, int quantity, int remainingQuantity)
        => new(productId, quantity, remainingQuantity, DateTime.UtcNow);
}