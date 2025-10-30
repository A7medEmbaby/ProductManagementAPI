using MediatR;
using ProductManagement.Application.Categories;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class ProductCreatedCountHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ICategoryRepository _categoryRepository;

    public ProductCreatedCountHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(notification.CategoryId, cancellationToken);

        if (category != null)
        {
            // Modify the domain entity
            category.IncrementProductCount();

            Console.WriteLine($"Product created: {notification.Name.Value} - Category {notification.CategoryId.Value} count will be incremented");
        }
    }
}