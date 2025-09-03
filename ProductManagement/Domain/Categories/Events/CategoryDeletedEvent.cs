using ProductManagement.Domain.Common;
using ProductManagement.Domain.ValueObjects;

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