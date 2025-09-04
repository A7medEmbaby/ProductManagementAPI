using MediatR;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers
{
    public class ProductDeletedCountHandler : INotificationHandler<ProductDeletedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductDeletedCountHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(notification.CategoryId, cancellationToken);
            if (category != null)
            {
                category.DecrementProductCount();
                await _categoryRepository.UpdateAsync(category, cancellationToken);
            }

            Console.WriteLine($"Product deleted - Category {notification.CategoryId.Value} count decremented");
        }
    }
}
