using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.Events;

public record CategoryDeletedEvent(
    CategoryId CategoryId,
    CategoryName Name,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CategoryDeletedEvent Create(CategoryId categoryId, CategoryName name)
        => new(categoryId, name, DateTime.UtcNow);
}