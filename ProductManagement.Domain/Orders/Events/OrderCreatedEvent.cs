using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders.ValueObjects;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Domain.Orders.Events;

public record OrderCreatedEvent(
    OrderId OrderId,
    UserId UserId,
    List<OrderItemData> Items,
    Money TotalAmount,
    DateTime OccurredAt
) : IDomainEvent
{
    public static OrderCreatedEvent Create(OrderId orderId, UserId userId, List<OrderItemData> items, Money totalAmount)
        => new(orderId, userId, items, totalAmount, DateTime.UtcNow);
}

public record OrderItemData(
    ProductId ProductId,
    ProductName ProductName,
    int Quantity,
    Money UnitPrice
);