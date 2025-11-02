using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record StockUpdatedEvent(
    ProductId ProductId,
    int OldQuantity,
    int NewQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static StockUpdatedEvent Create(ProductId productId, int oldQuantity, int newQuantity)
        => new(productId, oldQuantity, newQuantity, DateTime.UtcNow);
}