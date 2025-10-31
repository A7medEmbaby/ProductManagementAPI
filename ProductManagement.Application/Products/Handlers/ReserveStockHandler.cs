using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class ReserveStockHandler : IRequestHandler<ReserveStockCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public ReserveStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        // Get product
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Reserve stock
        product.ReserveStock(request.Quantity);

        // Save changes
        await _productRepository.UpdateAsync(product, cancellationToken);

        return product.ToResponse();
    }
}