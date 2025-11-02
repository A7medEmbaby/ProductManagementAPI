using MediatR;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Domain.Orders.Commands;

public record CancelOrderCommand(
    Guid OrderId,
    string Reason = "Cancelled by user"
) : IRequest<OrderResponse>
{
    public OrderId GetOrderId() => Domain.Orders.ValueObjects.OrderId.Create(OrderId);
}