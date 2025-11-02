using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Common.ValueObjects;

public sealed class 
    ProductId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    private ProductId() { } // For EF Core

    public static ProductId CreateUnique() => new ProductId(Guid.NewGuid());

    public static ProductId Create(Guid value) => new ProductId(value);

    public static ProductId Empty => new ProductId(Guid.Empty);

    public static implicit operator Guid(ProductId productId) => productId.Value;
}