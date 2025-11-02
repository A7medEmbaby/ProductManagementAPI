using MediatR;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders;

namespace ProductManagement.Application.Orders.Handlers;

public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        // Get order
        var order = await _orderRepository.GetByIdAsync(request.GetOrderId(), cancellationToken);
        if (order == null)
            throw new ArgumentException($"Order with ID {request.OrderId} not found");

        // Confirm order
        order.Confirm();

        // Save changes
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}