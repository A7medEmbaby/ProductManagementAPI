using MediatR;
using ProductManagement.Application.Orders.DTOs;
using ProductManagement.Domain.Cart.ValueObjects;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.Commands;

public record CreateOrderCommand(
    Guid UserId,
    List<OrderItemData> Items
) : IRequest<OrderResponse>
{
    public UserId GetUserId() => Domain.Cart.ValueObjects.UserId.Create(UserId);
}