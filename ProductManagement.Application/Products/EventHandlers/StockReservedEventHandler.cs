using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class StockReservedEventHandler : INotificationHandler<StockReservedEvent>
{
    public Task Handle(StockReservedEvent notification, CancellationToken cancellationToken)
    {
        // Handle stock reserved event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating read models
        // - Analytics

        Console.WriteLine($"Stock reserved: Product {notification.ProductId.Value}, Quantity: {notification.Quantity}, Available: {notification.AvailableQuantity}");

        return Task.CompletedTask;
    }
}