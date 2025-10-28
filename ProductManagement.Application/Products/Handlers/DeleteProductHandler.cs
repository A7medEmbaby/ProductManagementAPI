using MediatR;
using ProductManagement.Application.Products.Commands;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // Get product
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Delete product (raises domain event)
        product.Delete();

        // Remove from repository
        await _productRepository.DeleteAsync(product, cancellationToken);

        return Unit.Value;
    }
}