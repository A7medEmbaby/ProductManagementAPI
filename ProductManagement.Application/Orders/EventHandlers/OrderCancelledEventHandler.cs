using MediatR;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.EventHandlers;

public class OrderCancelledEventHandler : INotificationHandler<OrderCancelledEvent>
{
    public Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        // Handle order cancelled event
        // Could be used for:
        // - Releasing reserved stock
        // - Sending cancellation notifications
        // - Refunding payments
        // - Updating analytics

        Console.WriteLine($"Order cancelled: {notification.OrderId.Value}, Reason: {notification.Reason}");

        return Task.CompletedTask;
    }
}