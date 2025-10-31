using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class StockReleasedEventHandler : INotificationHandler<StockReleasedEvent>
{
    public Task Handle(StockReleasedEvent notification, CancellationToken cancellationToken)
    {
        // Handle stock released event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating read models
        // - Analytics

        Console.WriteLine($"Stock released: Product {notification.ProductId.Value}, Quantity: {notification.Quantity}, Available: {notification.AvailableQuantity}");

        return Task.CompletedTask;
    }
}