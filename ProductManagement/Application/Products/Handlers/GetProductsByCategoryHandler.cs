using MediatR;
using ProductManagement.Application.Products.Queries;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetByCategoryIdAsync(request.GetCategoryId(), cancellationToken);
        return products.ToResponse();
    }
}