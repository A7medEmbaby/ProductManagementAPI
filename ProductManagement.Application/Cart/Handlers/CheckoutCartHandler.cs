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

        // Capture response before checkout
        var response = cart.ToResponse();

        // Checkout (marks cart as checked out, raises CartCheckedOutEvent)
        cart.Checkout();

        // Save cart (publishes CartCheckedOutEvent to outbox, marks cart as CheckedOut)
        await _cartRepository.SaveAsync(cart, cancellationToken);

        // Cart clearing will be handled asynchronously by CartClearingConsumer
        // when it receives the OrderCreatedIntegrationEvent

        return response;
    }
}