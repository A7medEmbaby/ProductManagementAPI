using MediatR;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.EventHandlers;

public class OrderConfirmedEventHandler : INotificationHandler<OrderConfirmedEvent>
{
    public Task Handle(OrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Handle order confirmed event
        // Could be used for:
        // - Sending confirmation notifications
        // - Starting fulfillment process
        // - Updating inventory

        Console.WriteLine($"Order confirmed: {notification.OrderId.Value} for user {notification.UserId.Value}");

        return Task.CompletedTask;
    }
}