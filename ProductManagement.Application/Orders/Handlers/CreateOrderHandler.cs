using MediatR;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders;

namespace ProductManagement.Application.Orders.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Create order
        var order = Domain.Orders.Order.Create(request.GetUserId(), request.Items);

        // Save order
        await _orderRepository.AddAsync(order, cancellationToken);

        return order.ToResponse();
    }
}