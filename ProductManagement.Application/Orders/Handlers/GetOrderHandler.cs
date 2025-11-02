using MediatR;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Application.Orders.Queries;
using ProductManagement.Domain.Orders;

namespace ProductManagement.Application.Orders.Handlers;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderResponse?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.GetOrderId(), cancellationToken);
        return order?.ToResponse();
    }
}