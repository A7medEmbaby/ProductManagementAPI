using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class StockUpdatedEventHandler : INotificationHandler<StockUpdatedEvent>
{
    public Task Handle(StockUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle stock updated event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating inventory reports
        // - Analytics
        // - Audit trail

        Console.WriteLine($"Stock updated: Product {notification.ProductId.Value}, Old: {notification.OldQuantity}, New: {notification.NewQuantity}");

        return Task.CompletedTask;
    }
}