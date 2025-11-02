using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Cart.ValueObjects;

public sealed class CartItemId : ValueObject
{
    public Guid Value { get; private set; }

    private CartItemId(Guid value)
    {
        Value = value;
    }

    private CartItemId() { }

    public static CartItemId CreateUnique() => new(Guid.NewGuid());

    public static CartItemId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(CartItemId itemId) => itemId.Value;

    public override string ToString() => Value.ToString();
}