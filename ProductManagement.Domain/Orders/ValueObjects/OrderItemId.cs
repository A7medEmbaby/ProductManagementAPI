using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Orders.ValueObjects;

public sealed class OrderItemId : ValueObject
{
    public Guid Value { get; private set; }

    private OrderItemId(Guid value)
    {
        Value = value;
    }

    private OrderItemId() { }

    public static OrderItemId CreateUnique() => new(Guid.NewGuid());

    public static OrderItemId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(OrderItemId itemId) => itemId.Value;

    public override string ToString() => Value.ToString();
}