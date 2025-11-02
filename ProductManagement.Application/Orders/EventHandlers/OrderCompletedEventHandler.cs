using MediatR;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.EventHandlers;

public class OrderCompletedEventHandler : INotificationHandler<OrderCompletedEvent>
{
    public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        // Handle order completed event
        // Could be used for:
        // - Sending completion notifications
        // - Updating customer loyalty points
        // - Generating invoice
        // - Analytics

        Console.WriteLine($"Order completed: {notification.OrderId.Value} for user {notification.UserId.Value}");

        return Task.CompletedTask;
    }
}