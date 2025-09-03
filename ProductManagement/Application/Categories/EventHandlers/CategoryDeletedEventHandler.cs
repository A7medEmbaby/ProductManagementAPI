using MediatR;
using ProductManagement.Domain.Categories.Events;

namespace ProductManagement.Application.Categories.EventHandlers;

public class CategoryDeletedEventHandler : INotificationHandler<CategoryDeletedEvent>
{
    public Task Handle(CategoryDeletedEvent notification, CancellationToken cancellationToken)
    {
        // Handle category deleted event
        // Could be used for:
        // - Logging the deletion
        // - Cleaning up related data
        // - Notifying other systems
        // - Archiving information

        Console.WriteLine($"Category deleted: {notification.Name.Value} (ID: {notification.CategoryId.Value})");

        return Task.CompletedTask;
    }
}