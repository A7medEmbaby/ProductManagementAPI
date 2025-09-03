using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Products.Queries;
using ProductManagement.Application.Products.DTOs;
using ProductManagement.Domain.Products;

namespace ProductManagement.Application.Products.Handlers;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.GetValidPageNumber();
        var pageSize = request.GetValidPageSize();

        var products = await _productRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var totalCount = await _productRepository.GetTotalCountAsync(cancellationToken);

        var productResponses = products.ToResponse();

        return PagedResult<ProductResponse>.Create(productResponses, totalCount, pageNumber, pageSize);
    }
}