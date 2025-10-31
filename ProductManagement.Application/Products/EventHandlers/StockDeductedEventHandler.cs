using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class StockDeductedEventHandler : INotificationHandler<StockDeductedEvent>
{
    public Task Handle(StockDeductedEvent notification, CancellationToken cancellationToken)
    {
        // Handle stock deducted event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating inventory reports
        // - Analytics
        // - Triggering low stock alerts

        Console.WriteLine($"Stock deducted: Product {notification.ProductId.Value}, Quantity: {notification.Quantity}, Remaining: {notification.RemainingQuantity}");

        return Task.CompletedTask;
    }
}