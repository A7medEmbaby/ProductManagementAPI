using MediatR;
using ProductManagement.Application.Categories;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers;

public class ProductCategoryChangedCountHandler : INotificationHandler<ProductCategoryChangedEvent>
{
    private readonly ICategoryRepository _categoryRepository;

    public ProductCategoryChangedCountHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(ProductCategoryChangedEvent notification, CancellationToken cancellationToken)
    {
        var oldCategory = await _categoryRepository.GetByIdAsync(notification.OldCategoryId, cancellationToken);
        if (oldCategory != null)
        {
            oldCategory.DecrementProductCount();
        }

        var newCategory = await _categoryRepository.GetByIdAsync(notification.NewCategoryId, cancellationToken);
        if (newCategory != null)
        {
            newCategory.IncrementProductCount();
        }

        Console.WriteLine($"Product moved from category {notification.OldCategoryId.Value} to {notification.NewCategoryId.Value} - Counts will be updated");
    }
}