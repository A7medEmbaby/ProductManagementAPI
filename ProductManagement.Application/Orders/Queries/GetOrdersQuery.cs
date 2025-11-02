using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Orders.DTOs;

namespace ProductManagement.Application.Orders.Queries;

public record GetOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderResponse>>
{
    public int GetValidPageNumber() => PageNumber < 1 ? 1 : PageNumber;
    public int GetValidPageSize() => PageSize < 1 ? 10 : PageSize > 100 ? 100 : PageSize;
}