using MediatR;
using ProductManagement.Application.Common;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Application.Orders.Queries;
using ProductManagement.Domain.Orders;

namespace ProductManagement.Application.Orders.Handlers;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.GetValidPageNumber();
        var pageSize = request.GetValidPageSize();

        var orders = await _orderRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var totalCount = await _orderRepository.GetTotalCountAsync(cancellationToken);

        var orderResponses = orders.ToResponse();

        return PagedResult<OrderResponse>.Create(orderResponses, totalCount, pageNumber, pageSize);
    }
}