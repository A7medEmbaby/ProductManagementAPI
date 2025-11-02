using MediatR;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.Cart.DTOs;

namespace ProductManagement.Application.Cart.Handlers;

public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemFromCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CartResponse> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        // Get cart
        var cart = await _cartRepository.GetByUserIdAsync(request.GetUserId(), cancellationToken);
        if (cart == null)
            throw new ArgumentException($"Cart not found for user {request.UserId}");

        // Remove item
        cart.RemoveItem(request.GetCartItemId());

        // Save cart
        await _cartRepository.SaveAsync(cart, cancellationToken);

        return cart.ToResponse();
    }
}