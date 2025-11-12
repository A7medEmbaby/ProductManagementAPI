namespace ProductManagement.Application.Cart.DTOs;

public record CartResponse(
    Guid CartId,
    Guid UserId,
    string Status,
    List<CartItemResponse> Items,
    int ItemCount,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CartItemResponse(
    Guid ItemId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string Currency,
    decimal LineTotal
);

public static class CartExtensions
{
    public static CartResponse ToResponse(this Domain.Cart.Cart cart)
        => new(
            ((Domain.Cart.ValueObjects.CartId)cart.AggregateId).Value,
            cart.UserId.Value,
            cart.Status.ToString(),
            cart.Items.Select(ToItemResponse).ToList(),
            cart.ItemCount,
            cart.TotalAmount.Amount,
            cart.TotalAmount.Currency,
            cart.CreatedAt,
            cart.UpdatedAt
        );

    public static CartItemResponse ToItemResponse(this Domain.Cart.CartItem item)
        => new(
            item.Id.Value,
            item.ProductId.Value,
            item.ProductName.Value,
            item.Quantity,
            item.UnitPrice.Amount,
            item.UnitPrice.Currency,
            item.LineTotal.Amount
        );
}