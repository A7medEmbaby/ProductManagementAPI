using MediatR;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Application.Orders.Queries;

public record GetOrderQuery(Guid OrderId) : IRequest<OrderResponse?>
{
    public OrderId GetOrderId() => Domain.Orders.ValueObjects.OrderId.Create(OrderId);
}