using ProductManagement.Domain.Common.Models;

namespace ProductManagement.Domain.Orders.ValueObjects;

public sealed class OrderId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    private OrderId() { } // For EF Core

    public static OrderId CreateUnique() => new(Guid.NewGuid());

    public static OrderId Create(Guid value) => new(value);

    public static OrderId Empty => new(Guid.Empty);

    public static implicit operator Guid(OrderId orderId) => orderId.Value;
}