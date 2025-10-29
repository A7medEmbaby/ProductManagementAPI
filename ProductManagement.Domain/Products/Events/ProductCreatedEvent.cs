using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Products.Events;

public record ProductCreatedEvent(
    ProductId ProductId,
    ProductName Name,
    CategoryId CategoryId,
    Money Price,
    DateTime OccurredAt
) : IDomainEvent
{
    public static ProductCreatedEvent Create(ProductId productId, ProductName name, CategoryId categoryId, Money price)
        => new(productId, name, categoryId, price, DateTime.UtcNow);
}