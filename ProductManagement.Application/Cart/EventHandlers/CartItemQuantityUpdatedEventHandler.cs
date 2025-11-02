using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Products;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartItemQuantityUpdatedEventHandler : INotificationHandler<CartItemQuantityUpdatedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartItemQuantityUpdatedEventHandler> _logger;

    public CartItemQuantityUpdatedEventHandler(
        IProductRepository productRepository,
        ILogger<CartItemQuantityUpdatedEventHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(CartItemQuantityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cart item quantity updated: User {UserId}, Product {ProductId}, Old: {OldQuantity}, New: {NewQuantity}",
            notification.UserId.Value,
            notification.ProductId.Value,
            notification.OldQuantity,
            notification.NewQuantity);

        var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);
        if (product == null)
        {
            _logger.LogWarning(
                "Product not found for stock adjustment. ProductId: {ProductId}",
                notification.ProductId.Value);
            return;
        }

        // Calculate the difference
        var quantityDifference = notification.NewQuantity - notification.OldQuantity;

        if (quantityDifference > 0)
        {
            // Quantity increased - reserve additional stock
            product.ReserveStock(quantityDifference);
            _logger.LogInformation(
                "Additional stock reserved. ProductId: {ProductId}, Quantity: {Quantity}, Available: {Available}",
                notification.ProductId.Value,
                quantityDifference,
                product.Stock.AvailableQuantity);
        }
        else if (quantityDifference < 0)
        {
            // Quantity decreased - release excess stock
            var releaseQuantity = Math.Abs(quantityDifference);
            product.ReleaseStock(releaseQuantity);
            _logger.LogInformation(
                "Excess stock released. ProductId: {ProductId}, Quantity: {Quantity}, Available: {Available}",
                notification.ProductId.Value,
                releaseQuantity,
                product.Stock.AvailableQuantity);
        }

        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
