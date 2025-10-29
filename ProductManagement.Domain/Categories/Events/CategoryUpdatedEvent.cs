using ProductManagement.Domain.Categories.ValueObjects;
using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.Events;

public record CategoryUpdatedEvent(
    CategoryId CategoryId,
    CategoryName OldName,
    CategoryName NewName,
    DateTime OccurredAt
) : IDomainEvent
{
    public static CategoryUpdatedEvent Create(CategoryId categoryId, CategoryName oldName, CategoryName newName)
        => new(categoryId, oldName, newName, DateTime.UtcNow);
}