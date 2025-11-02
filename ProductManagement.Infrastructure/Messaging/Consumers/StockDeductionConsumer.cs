using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Products;
using ProductManagement.Application.Settings;
using ProductManagement.Domain.Common.ValueObjects;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class StockDeductionConsumer : RabbitMQConsumerBase<OrderCreatedIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQSettings _settings;

    protected override string QueueName => _settings.Queues.StockDeduction;
    protected override string ExchangeName => _settings.Exchanges.OrderEvents;
    protected override string RoutingKey => _settings.RoutingKeys.OrderCreated;

    public StockDeductionConsumer(
        RabbitMQConnectionFactory connectionFactory,
        IOptions<RabbitMQSettings> settings,
        IServiceProvider serviceProvider,
        ILogger<StockDeductionConsumer> logger)
        : base(connectionFactory, settings, logger)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    protected override async Task ProcessMessageAsync(OrderCreatedIntegrationEvent message)
    {
        Logger.LogInformation(
            "Processing stock deduction for order. OrderId: {OrderId}, Items: {ItemCount}",
            message.OrderId,
            message.Items.Count);

        using var scope = _serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        var failedItems = new List<string>();

        foreach (var item in message.Items)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(
                    ProductId.Create(item.ProductId));

                if (product == null)
                {
                    Logger.LogWarning(
                        "Product not found for stock deduction. ProductId: {ProductId}",
                        item.ProductId);
                    failedItems.Add($"Product {item.ProductId} not found");
                    continue;
                }

                // Check if sufficient stock
                if (!product.HasAvailableStock(item.Quantity))
                {
                    Logger.LogWarning(
                        "Insufficient stock for product. ProductId: {ProductId}, Required: {Required}, Available: {Available}",
                        item.ProductId,
                        item.Quantity,
                        product.Stock.AvailableQuantity);
                    failedItems.Add($"Insufficient stock for product {item.ProductName}");
                    continue;
                }

                // Deduct stock
                product.DeductStock(item.Quantity);
                await productRepository.UpdateAsync(product);

                Logger.LogInformation(
                    "Stock deducted successfully. ProductId: {ProductId}, Quantity: {Quantity}",
                    item.ProductId,
                    item.Quantity);
            }
            catch (Exception ex)
            {
                Logger.LogError(
                    ex,
                    "Error deducting stock for product. ProductId: {ProductId}",
                    item.ProductId);
                failedItems.Add($"Error deducting stock for product {item.ProductName}");
            }
        }

        if (failedItems.Any())
        {
            var errorMessage = string.Join("; ", failedItems);
            Logger.LogError(
                "Stock deduction completed with errors for order {OrderId}: {Errors}",
                message.OrderId,
                errorMessage);
            throw new InvalidOperationException($"Stock deduction failed: {errorMessage}");
        }

        Logger.LogInformation(
            "Stock deduction completed successfully for order. OrderId: {OrderId}",
            message.OrderId);
    }
}