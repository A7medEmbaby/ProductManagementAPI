using MediatR;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.Cart.DTOs;
using ProductManagement.Application.Products;

namespace ProductManagement.Application.Cart.Handlers;

public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public AddItemToCartHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.GetProductId(), cancellationToken);
        if (product == null)
            throw new ArgumentException($"Product with ID {request.ProductId} not found");

        // Check stock availability
        if (!product.HasAvailableStock(request.Quantity))
            throw new InvalidOperationException(
                $"Insufficient stock. Available: {product.Stock.AvailableQuantity}, Requested: {request.Quantity}");

        // Get or create cart
        var cart = await _cartRepository.GetByUserIdAsync(request.GetUserId(), cancellationToken);
        if (cart == null)
        {
            cart = Domain.Cart.Cart.Create(request.GetUserId());
        }

        // Add item to cart
        cart.AddItem((Domain.Common.ValueObjects.ProductId)product.AggregateId, product.Name, request.Quantity, product.Price);

        // Save cart
        await _cartRepository.SaveAsync(cart, cancellationToken);

        return cart.ToResponse();
    }
}