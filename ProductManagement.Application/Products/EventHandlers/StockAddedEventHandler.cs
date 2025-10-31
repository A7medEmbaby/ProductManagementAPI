using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class StockAddedEventHandler : INotificationHandler<StockAddedEvent>
{
    public Task Handle(StockAddedEvent notification, CancellationToken cancellationToken)
    {
        // Handle stock added event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating inventory reports
        // - Analytics

        Console.WriteLine($"Stock added: Product {notification.ProductId.Value}, Quantity: {notification.Quantity}, Total: {notification.TotalQuantity}");

        return Task.CompletedTask;
    }
}