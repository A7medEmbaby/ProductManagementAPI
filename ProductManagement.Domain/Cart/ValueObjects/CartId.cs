using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Cart.ValueObjects;

public sealed class CartId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private CartId(Guid value)
    {
        Value = value;
    }

    private CartId() { } // For EF Core

    public static CartId CreateUnique() => new(Guid.NewGuid());

    public static CartId Create(Guid value) => new(value);

    public static CartId Empty => new(Guid.Empty);

    public static implicit operator Guid(CartId cartId) => cartId.Value;
}