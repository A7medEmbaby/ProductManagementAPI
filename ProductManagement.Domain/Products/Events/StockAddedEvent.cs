using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record StockAddedEvent(
    ProductId ProductId,
    int Quantity,
    int TotalQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static StockAddedEvent Create(ProductId productId, int quantity, int totalQuantity)
        => new(productId, quantity, totalQuantity, DateTime.UtcNow);
}