using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class ProductCategoryChangedEventHandler : INotificationHandler<ProductCategoryChangedEvent>
{
    public Task Handle(ProductCategoryChangedEvent notification, CancellationToken cancellationToken)
    {
        // Handle product category changed event
        // Could be used for:
        // - Updating category statistics
        // - Logging the change
        // - Notifying interested parties
        // - Triggering business rules

        Console.WriteLine($"Product {notification.ProductId.Value} moved from category {notification.OldCategoryId.Value} to {notification.NewCategoryId.Value}");

        return Task.CompletedTask;
    }
}