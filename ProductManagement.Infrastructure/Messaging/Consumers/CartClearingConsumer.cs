using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Settings;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class CartClearingConsumer : RabbitMQConsumerBase<OrderCreatedIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQSettings _settings;

    protected override string QueueName => _settings.Queues.CartClearing;
    protected override string ExchangeName => _settings.Exchanges.OrderEvents;
    protected override string RoutingKey => _settings.RoutingKeys.OrderCreated;

    public CartClearingConsumer(
        RabbitMQConnectionFactory connectionFactory,
        IOptions<RabbitMQSettings> settings,
        IServiceProvider serviceProvider,
        ILogger<CartClearingConsumer> logger)
        : base(connectionFactory, settings, logger)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    protected override async Task ProcessMessageAsync(OrderCreatedIntegrationEvent message)
    {
        Logger.LogInformation(
            "Processing cart clearing for user. UserId: {UserId}, OrderId: {OrderId}",
            message.UserId,
            message.OrderId);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Clear cart command
        var command = new ClearCartCommand(message.UserId);

        // Execute command
        await mediator.Send(command);

        Logger.LogInformation(
            "Cart cleared successfully. UserId: {UserId}, OrderId: {OrderId}",
            message.UserId,
            message.OrderId);
    }
}