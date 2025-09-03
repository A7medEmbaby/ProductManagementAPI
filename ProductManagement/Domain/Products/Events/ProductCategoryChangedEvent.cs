using ProductManagement.Domain.Common;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record ProductCategoryChangedEvent(
    ProductId ProductId,
    CategoryId OldCategoryId,
    CategoryId NewCategoryId,
    DateTime OccurredAt
) : IDomainEvent
{
    public static ProductCategoryChangedEvent Create(ProductId productId, CategoryId oldCategoryId, CategoryId newCategoryId)
        => new(productId, oldCategoryId, newCategoryId, DateTime.UtcNow);
}