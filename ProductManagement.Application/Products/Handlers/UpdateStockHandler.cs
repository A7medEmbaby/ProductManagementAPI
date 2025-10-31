using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public UpdateStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        // Get product
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Update stock
        product.UpdateStock(request.NewQuantity);

        // Save changes
        await _productRepository.UpdateAsync(product, cancellationToken);

        return product.ToResponse();
    }
}