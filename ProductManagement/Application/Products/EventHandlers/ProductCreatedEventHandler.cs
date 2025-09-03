using MediatR;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle product created event
        // Could be used for:
        // - Logging
        // - Sending notifications
        // - Updating read models
        // - Triggering other business processes

        Console.WriteLine($"Product created: {notification.Name.Value} (ID: {notification.ProductId.Value}) in category {notification.CategoryId.Value}");

        return Task.CompletedTask;
    }
}