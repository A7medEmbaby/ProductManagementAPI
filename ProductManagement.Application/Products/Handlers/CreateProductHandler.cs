using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;
using ProductManagement.Application.Categories;

namespace ProductManagement.Application.Products.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate category exists
        var categoryExists = await _categoryRepository.ExistsAsync(request.GetCategoryId(), cancellationToken);
        if (!categoryExists)
            throw new ArgumentException($"Category with ID {request.CategoryId} does not exist");

        // Validate initial stock
        if (request.InitialStock < 0)
            throw new ArgumentException("Initial stock cannot be negative");

        // Create product with initial stock
        var product = Product.Create(
            request.GetProductName(),
            request.GetCategoryId(),
            request.GetPrice(),
            request.InitialStock
        );

        // Save product
        await _productRepository.AddAsync(product, cancellationToken);

        return product.ToResponse();
    }
}