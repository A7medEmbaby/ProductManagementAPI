using MediatR;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers
{
    public class ProductCategoryChangedCountHandler : INotificationHandler<ProductCategoryChangedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductCategoryChangedCountHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(ProductCategoryChangedEvent notification, CancellationToken cancellationToken)
        {
            // Decrement old category count
            var oldCategory = await _categoryRepository.GetByIdAsync(notification.OldCategoryId, cancellationToken);
            if (oldCategory != null)
            {
                oldCategory.DecrementProductCount();
                await _categoryRepository.UpdateAsync(oldCategory, cancellationToken);
            }

            // Increment new category count
            var newCategory = await _categoryRepository.GetByIdAsync(notification.NewCategoryId, cancellationToken);
            if (newCategory != null)
            {
                newCategory.IncrementProductCount();
                await _categoryRepository.UpdateAsync(newCategory, cancellationToken);
            }

            Console.WriteLine($"Product moved from category {notification.OldCategoryId.Value} to {notification.NewCategoryId.Value} - Counts updated");
        }
    }
}
