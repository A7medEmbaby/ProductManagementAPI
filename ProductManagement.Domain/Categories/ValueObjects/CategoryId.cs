using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Categories.ValueObjects;

public sealed class CategoryId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    private CategoryId() { } // For EF Core

    public static CategoryId CreateUnique() => new(Guid.NewGuid());

    public static CategoryId Create(Guid value) => new(value);

    public static CategoryId Empty => new(Guid.Empty);

    public static implicit operator Guid(CategoryId categoryId) => categoryId.Value;
}