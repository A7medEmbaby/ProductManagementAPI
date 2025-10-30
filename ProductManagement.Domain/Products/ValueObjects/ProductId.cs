using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products.ValueObjects;

public sealed class ProductId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    private ProductId() { } // For EF Core

    public static ProductId CreateUnique() => new(Guid.NewGuid());

    public static ProductId Create(Guid value) => new(value);

    public static ProductId Empty => new(Guid.Empty);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(ProductId productId) => productId.Value;
}