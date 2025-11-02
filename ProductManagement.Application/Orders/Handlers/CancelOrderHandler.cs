using MediatR;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders;
using ProductManagement.Domain.Orders.Commands;

namespace ProductManagement.Application.Orders.Handlers;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        // Get order
        var order = await _orderRepository.GetByIdAsync(request.GetOrderId(), cancellationToken);
        if (order == null)
            throw new ArgumentException($"Order with ID {request.OrderId} not found");

        // Cancel order
        order.Cancel(request.Reason);

        // Save changes
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}