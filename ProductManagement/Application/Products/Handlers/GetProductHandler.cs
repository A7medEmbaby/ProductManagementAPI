using MediatR;
using ProductManagement.Application.Products.Queries;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class GetProductHandler : IRequestHandler<GetProductQuery, ProductResponse?>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        return product?.ToResponse();
    }
}