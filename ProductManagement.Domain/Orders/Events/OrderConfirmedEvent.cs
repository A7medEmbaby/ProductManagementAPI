using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Domain.Orders.Events;

public record OrderConfirmedEvent(
    OrderId OrderId,
    UserId UserId,
    DateTime OccurredAt
) : IDomainEvent
{
    public static OrderConfirmedEvent Create(OrderId orderId, UserId userId)
        => new(orderId, userId, DateTime.UtcNow);
}