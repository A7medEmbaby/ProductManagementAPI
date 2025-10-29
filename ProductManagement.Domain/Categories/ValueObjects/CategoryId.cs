using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.ValueObjects;

public sealed class CategoryId : AggregateRootId<Guid>
{
    private CategoryId(Guid value) : base(value) { }

    private CategoryId() : base() { } // For EF Core

    public static CategoryId New() => new(Guid.NewGuid());
    
    public static CategoryId Create(Guid value) => new(value);

    public static CategoryId Empty => new(Guid.Empty);

    // Implicit conversions for convenience
    public static implicit operator Guid(CategoryId categoryId) => categoryId.Value;
}