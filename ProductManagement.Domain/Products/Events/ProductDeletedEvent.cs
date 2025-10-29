using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record ProductDeletedEvent(
    ProductId ProductId,
    CategoryId CategoryId,
    DateTime OccurredAt
) : IDomainEvent
{
    public static ProductDeletedEvent Create(ProductId productId, CategoryId categoryId)
        => new(productId, categoryId, DateTime.UtcNow);
}