using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Products;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartItemAddedEventHandler : INotificationHandler<CartItemAddedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartItemAddedEventHandler> _logger;

    public CartItemAddedEventHandler(
        IProductRepository productRepository,
        ILogger<CartItemAddedEventHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(CartItemAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Item added to cart: User {UserId}, Product {ProductId}, Quantity: {Quantity}",
            notification.UserId.Value,
            notification.ProductId.Value,
            notification.Quantity);

        // Reserve stock for the added item
        var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);
        if (product == null)
        {
            _logger.LogWarning(
                "Product not found for stock reservation. ProductId: {ProductId}",
                notification.ProductId.Value);
            return;
        }

        // Reserve the stock
        product.ReserveStock(notification.Quantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation(
            "Stock reserved for product. ProductId: {ProductId}, Quantity: {Quantity}, Available: {Available}",
            notification.ProductId.Value,
            notification.Quantity,
            product.Stock.AvailableQuantity);
    }
}
