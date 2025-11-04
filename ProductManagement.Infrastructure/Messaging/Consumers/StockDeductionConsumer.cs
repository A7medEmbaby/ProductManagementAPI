using MassTransit;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Products;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class StockDeductionConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<StockDeductionConsumer> _logger;

    public StockDeductionConsumer(
        IProductRepository productRepository,
        ILogger<StockDeductionConsumer> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Processing stock deduction for order. OrderId: {OrderId}, Items: {ItemCount}",
            message.OrderId,
            message.Items.Count);

        var failedItems = new List<string>();

        foreach (var item in message.Items)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(
                    ProductId.Create(item.ProductId));

                if (product == null)
                {
                    _logger.LogWarning(
                        "Product not found for stock deduction. ProductId: {ProductId}",
                        item.ProductId);
                    failedItems.Add($"Product {item.ProductId} not found");
                    continue;
                }

                // Check if sufficient stock
                if (!product.HasAvailableStock(item.Quantity))
                {
                    _logger.LogWarning(
                        "Insufficient stock for product. ProductId: {ProductId}, Required: {Required}, Available: {Available}",
                        item.ProductId,
                        item.Quantity,
                        product.Stock.AvailableQuantity);
                    failedItems.Add($"Insufficient stock for product {item.ProductName}");
                    continue;
                }

                // Deduct stock
                product.DeductStock(item.Quantity);
                await _productRepository.UpdateAsync(product);

                _logger.LogInformation(
                    "Stock deducted successfully. ProductId: {ProductId}, Quantity: {Quantity}",
                    item.ProductId,
                    item.Quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deducting stock for product. ProductId: {ProductId}",
                    item.ProductId);
                failedItems.Add($"Error deducting stock for product {item.ProductName}");
            }
        }

        if (failedItems.Any())
        {
            var errorMessage = string.Join("; ", failedItems);
            _logger.LogError(
                "Stock deduction completed with errors for order {OrderId}: {Errors}",
                message.OrderId,
                errorMessage);
            throw new InvalidOperationException($"Stock deduction failed: {errorMessage}");
        }

        _logger.LogInformation(
            "Stock deduction completed successfully for order. OrderId: {OrderId}",
            message.OrderId);
    }
}
