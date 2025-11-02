using ProductManagement.Domain.Orders.ValueObjects;

namespace ProductManagement.Application.Orders.DTOs;

public record OrderResponse(
    Guid OrderId,
    Guid UserId,
    List<OrderItemResponse> Items,
    OrderStatus Status,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? CompletedAt,
    DateTime? CancelledAt
);

public record OrderItemResponse(
    Guid ItemId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string Currency,
    decimal LineTotal
);

public static class OrderExtensions
{
    public static OrderResponse ToResponse(this Domain.Orders.Order order)
        => new(
            ((Domain.Orders.ValueObjects.OrderId)order.AggregateId).Value,
            order.UserId.Value,
            order.Items.Select(ToItemResponse).ToList(),
            order.Status,
            order.TotalAmount.Amount,
            order.TotalAmount.Currency,
            order.CreatedAt,
            order.UpdatedAt,
            order.CompletedAt,
            order.CancelledAt
        );

    public static OrderItemResponse ToItemResponse(this Domain.Orders.OrderItem item)
        => new(
            item.Id.Value,
            item.ProductId.Value,
            item.ProductName.Value,
            item.Quantity,
            item.UnitPrice.Amount,
            item.UnitPrice.Currency,
            item.LineTotal.Amount
        );

    public static List<OrderResponse> ToResponse(this IEnumerable<Domain.Orders.Order> orders)
        => orders.Select(ToResponse).ToList();
}