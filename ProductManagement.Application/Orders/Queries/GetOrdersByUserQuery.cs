using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;

namespace ProductManagement.Application.Orders.Queries;

public record GetOrdersByUserQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderResponse>>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
    public int GetValidPageNumber() => PageNumber < 1 ? 1 : PageNumber;
    public int GetValidPageSize() => PageSize < 1 ? 10 : PageSize > 100 ? 100 : PageSize;
}