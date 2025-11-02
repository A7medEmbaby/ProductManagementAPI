using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Products;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartItemRemovedEventHandler : INotificationHandler<CartItemRemovedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartItemRemovedEventHandler> _logger;

    public CartItemRemovedEventHandler(
        IProductRepository productRepository,
        ILogger<CartItemRemovedEventHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(CartItemRemovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Item removed from cart: User {UserId}, Product {ProductId}, Quantity: {Quantity}",
            notification.UserId.Value,
            notification.ProductId.Value,
            notification.Quantity);

        // Release reserved stock
        var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);
        if (product == null)
        {
            _logger.LogWarning(
                "Product not found for stock release. ProductId: {ProductId}",
                notification.ProductId.Value);
            return;
        }

        // Release the stock
        product.ReleaseStock(notification.Quantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation(
            "Stock released for product. ProductId: {ProductId}, Quantity: {Quantity}, Available: {Available}",
            notification.ProductId.Value,
            notification.Quantity,
            product.Stock.AvailableQuantity);
    }
}
