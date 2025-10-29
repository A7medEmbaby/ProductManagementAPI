using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.Events;

public record CategoryCreatedEvent(
    CategoryId CategoryId,
    CategoryName Name,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CategoryCreatedEvent Create(CategoryId categoryId, CategoryName name)
        => new(categoryId, name, DateTime.UtcNow);
}