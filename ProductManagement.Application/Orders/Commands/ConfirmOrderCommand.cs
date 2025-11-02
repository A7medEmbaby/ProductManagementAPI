using MediatR;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Application.Orders.Commands;

public record ConfirmOrderCommand(Guid OrderId) : IRequest<OrderResponse>
{
    public OrderId GetOrderId() => Domain.Orders.ValueObjects.OrderId.Create(OrderId);
}