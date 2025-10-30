using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Application.Categories;

namespace ProductManagement.Application.Products.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // Get product
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Validate new category exists
        var categoryExists = await _categoryRepository.ExistsAsync(request.GetCategoryId(), cancellationToken);
        if (!categoryExists)
            throw new ArgumentException($"Category with ID {request.CategoryId} does not exist");

        // Update product properties
        product.UpdateName(request.GetProductName());
        product.UpdatePrice(request.GetPrice());

        // Change category if different (this will trigger ProductCategoryChangedEvent)
        if (product.CategoryId.Value != request.CategoryId)
        {
            product.ChangeCategory(request.GetCategoryId());
        }

        // Save changes
        await _productRepository.UpdateAsync(product, cancellationToken);

        return product.ToResponse();
    }
}