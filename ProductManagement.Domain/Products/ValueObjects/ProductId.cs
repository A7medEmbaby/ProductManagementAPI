using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Products.ValueObjects;

public sealed class ProductId : AggregateRootId<Guid>
{
    private ProductId(Guid value) : base(value) { }

    private ProductId() : base() { } // For EF Core

    public static ProductId New() => new(Guid.NewGuid());
    
    public static ProductId Create(Guid value) => new(value);

    public static ProductId Empty => new(Guid.Empty);

    // Implicit conversions for convenience
    public static implicit operator Guid(ProductId productId) => productId.Value;
}