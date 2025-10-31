using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record StockReservedEvent(
    ProductId ProductId,
    int Quantity,
    int AvailableQuantity,
    DateTime OccurredAt
) : IDomainEvent
{
    public static StockReservedEvent Create(ProductId productId, int quantity, int availableQuantity)
        => new(productId, quantity, availableQuantity, DateTime.UtcNow);
}