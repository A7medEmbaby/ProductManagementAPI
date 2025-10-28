using MediatR;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Products.Events;

namespace ProductManagement.Application.Products.EventHandlers
{
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
                category.IncrementProductCount();
                await _categoryRepository.UpdateAsync(category, cancellationToken);
            }

            Console.WriteLine($"Product created: {notification.Name.Value} - Category count updated");
        }
    }
}
