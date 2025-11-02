using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Application.Orders.Queries;
using ProductManagement.Domain.Orders;

namespace ProductManagement.Application.Orders.Handlers;

public class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUserQuery, PagedResult<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderResponse>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.GetValidPageNumber();
        var pageSize = request.GetValidPageSize();

        var orders = await _orderRepository.GetPagedByUserIdAsync(
            request.GetUserId(),
            pageNumber,
            pageSize,
            cancellationToken);

        var totalCount = await _orderRepository.GetTotalCountByUserIdAsync(
            request.GetUserId(),
            cancellationToken);

        var orderResponses = orders.ToResponse();

        return PagedResult<OrderResponse>.Create(orderResponses, totalCount, pageNumber, pageSize);
    }
}