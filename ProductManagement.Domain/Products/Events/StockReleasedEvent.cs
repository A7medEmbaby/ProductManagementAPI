using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record StockReleasedEvent(
    ProductId ProductId,
    int Quantity,
    int AvailableQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static StockReleasedEvent Create(ProductId productId, int quantity, int availableQuantity)
        => new(productId, quantity, availableQuantity, DateTime.UtcNow);
}