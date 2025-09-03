using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Products.DTOs;

namespace ProductManagement.Application.Products.Queries;

public record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<ProductResponse>>
{
    public int GetValidPageNumber() => PageNumber < 1 ? 1 : PageNumber;
    public int GetValidPageSize() => PageSize < 1 ? 10 : PageSize > 100 ? 100 : PageSize;
}