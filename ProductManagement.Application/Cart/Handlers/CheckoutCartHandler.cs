using MediatR;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.Cart.DTOs;

namespace ProductManagement.Application.Cart.Handlers;

public class CheckoutCartHandler : IRequestHandler<CheckoutCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;

    public CheckoutCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CartResponse> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        // Get cart
        var cart = await _cartRepository.GetByUserIdAsync(request.GetUserId(), cancellationToken);
        if (cart == null)
            throw new ArgumentException($"Cart not found for user {request.UserId}");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cannot checkout an empty cart");

        // Capture response before clearing cart
        var response = cart.ToResponse();

        // Checkout (raises CartCheckedOutEvent)
        cart.Checkout();

        // Save cart (publishes CartCheckedOutEvent, commits to outbox)
        await _cartRepository.SaveAsync(cart, cancellationToken);

        // Clear cart items (raises CartClearedEvent to release stock)
        cart.Clear();

        // Save again (publishes CartClearedEvent, releases reserved stock)
        await _cartRepository.SaveAsync(cart, cancellationToken);

        // Delete empty cart from repository (free memory)
        await _cartRepository.DeleteAsync(request.GetUserId(), cancellationToken);

        return response;
    }
}