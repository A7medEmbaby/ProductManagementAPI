using MediatR;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Application.Products;

namespace ProductManagement.Application.Cart.Handlers;

public class UpdateCartItemQuantityHandler : IRequestHandler<UpdateCartItemQuantityCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public UpdateCartItemQuantityHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        // Get cart
        var cart = await _cartRepository.GetByUserIdAsync(request.GetUserId(), cancellationToken);
        if (cart == null)
            throw new ArgumentException($"Cart not found for user {request.UserId}");

        // Get cart item to validate product stock
        var cartItem = cart.GetItem(request.GetCartItemId());
        if (cartItem == null)
            throw new ArgumentException($"Cart item with ID {request.CartItemId} not found");

        // Validate stock availability for new quantity
        var product = await _productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {cartItem.ProductId} not found");

        if (!product.HasAvailableStock(request.NewQuantity))
            throw new InvalidOperationException(
                $"Insufficient stock. Available: {product.Stock.AvailableQuantity}, Requested: {request.NewQuantity}");

        // Update quantity
        cart.UpdateItemQuantity(request.GetCartItemId(), request.NewQuantity);

        // Save cart
        await _cartRepository.SaveAsync(cart, cancellationToken);

        return cart.ToResponse();
    }
}