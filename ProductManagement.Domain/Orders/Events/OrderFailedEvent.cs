using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Common.Models;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Domain.Orders.Events;

public record OrderFailedEvent(
    OrderId OrderId,
    UserId UserId,
    string Reason,
    DateTime OccurredAt
) : IDomainEvent
{
    public static OrderFailedEvent Create(OrderId orderId, UserId userId, string reason)
        => new(orderId, userId, reason, DateTime.UtcNow);
}