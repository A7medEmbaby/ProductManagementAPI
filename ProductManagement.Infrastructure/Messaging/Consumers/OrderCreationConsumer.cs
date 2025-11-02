using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Application.Settings;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders.Events;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class OrderCreationConsumer : RabbitMQConsumerBase<CartCheckedOutIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQSettings _settings;

    protected override string QueueName => _settings.Queues.OrderCreation;
    protected override string ExchangeName => _settings.Exchanges.CartEvents;
    protected override string RoutingKey => _settings.RoutingKeys.CartCheckedOut;

    public OrderCreationConsumer(
        RabbitMQConnectionFactory connectionFactory,
        IOptions<RabbitMQSettings> settings,
        IServiceProvider serviceProvider,
        ILogger<OrderCreationConsumer> logger)
        : base(connectionFactory, settings, logger)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    protected override async Task ProcessMessageAsync(CartCheckedOutIntegrationEvent message)
    {
        Logger.LogInformation(
            "Processing cart checkout for order creation. UserId: {UserId}, CartId: {CartId}",
            message.UserId,
            message.CartId);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Map integration event to order items
        var orderItems = message.Items.Select(item => new OrderItemData(
            ProductId.Create(item.ProductId),
            new ProductName(item.ProductName),
            item.Quantity,
            Money.Create(item.UnitPrice, item.Currency)
        )).ToList();

        // Create order command
        var command = new CreateOrderCommand(message.UserId, orderItems);

        // Execute command
        var result = await mediator.Send(command);

        Logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}, UserId: {UserId}",
            result.OrderId,
            message.UserId);
    }
}