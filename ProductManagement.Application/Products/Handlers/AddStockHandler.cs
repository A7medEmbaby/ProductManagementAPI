using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class AddStockHandler : IRequestHandler<AddStockCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public AddStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        // Get product
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Add stock
        product.AddStock(request.Quantity);

        // Save changes
        await _productRepository.UpdateAsync(product, cancellationToken);

        return product.ToResponse();
    }
}