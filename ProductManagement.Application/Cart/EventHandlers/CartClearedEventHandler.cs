using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Products;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartClearedEventHandler : INotificationHandler<CartClearedEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CartClearedEventHandler> _logger;

    public CartClearedEventHandler(
        IProductRepository productRepository,
        ILogger<CartClearedEventHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Handle(CartClearedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cart cleared: User {UserId}, Items: {ItemCount}",
            notification.UserId.Value,
            notification.Items.Count);

        // Release stock for all items
        foreach (var item in notification.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning(
                    "Product not found for stock release. ProductId: {ProductId}",
                    item.ProductId.Value);
                continue;
            }

            product.ReleaseStock(item.Quantity);
            await _productRepository.UpdateAsync(product, cancellationToken);

            _logger.LogInformation(
                "Stock released for product. ProductId: {ProductId}, Quantity: {Quantity}, Available: {Available}",
                item.ProductId.Value,
                item.Quantity,
                product.Stock.AvailableQuantity);
        }
    }
}
