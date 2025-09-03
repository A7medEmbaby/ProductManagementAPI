using ProductManagement.Domain.Common;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record ProductUpdatedEvent(
    ProductId ProductId,
    ProductName Name,
    Money Price,
    DateTime OccurredAt
) : IDomainEvent
{
    public static ProductUpdatedEvent Create(ProductId productId, ProductName name, Money price)
        => new(productId, name, price, DateTime.UtcNow);
}