using MediatR;
using ProductManagement.Application.Categories.Commands;
using ProductManagement.Domain.Categories;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Categories.Handlers;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // Get category
        var category = await _categoryRepository.GetByIdAsync(request.GetCategoryId(), cancellationToken);
        if (category == null)
            throw new ArgumentException($"Category with ID {request.CategoryId} not found");

        // Check if category has products
        var productsInCategory = await _productRepository.GetByCategoryIdAsync(request.GetCategoryId(), cancellationToken);
        if (productsInCategory.Any())
            throw new InvalidOperationException($"Cannot delete category '{category.Name.Value}' because it contains {productsInCategory.Count} product(s)");

        // Delete category (raises domain event)
        category.Delete();

        // Remove from repository
        await _categoryRepository.DeleteAsync(category, cancellationToken);

        return Unit.Value;
    }
}