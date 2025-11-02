using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Messaging;
using ProductManagement.Application.Settings;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(
        IMessageBus messageBus,
        IOptions<RabbitMQSettings> settings,
        ILogger<OrderCreatedEventHandler> logger)
    {
        _messageBus = messageBus;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Order created: OrderId={OrderId}, UserId={UserId}, Items={ItemCount}, Total={Total}",
            notification.OrderId.Value,
            notification.UserId.Value,
            notification.Items.Count,
            notification.TotalAmount);

        // Map to integration event
        var integrationEvent = new OrderCreatedIntegrationEvent
        {
            EventId = Guid.NewGuid(),
            OrderId = notification.OrderId.Value,
            UserId = notification.UserId.Value,
            Items = notification.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId.Value,
                ProductName = item.ProductName.Value,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice.Amount,
                Currency = item.UnitPrice.Currency
            }).ToList(),
            TotalAmount = notification.TotalAmount.Amount,
            Currency = notification.TotalAmount.Currency,
            OccurredAt = notification.OccurredAt
        };

        // Publish to RabbitMQ
        await _messageBus.PublishAsync(
            integrationEvent,
            _settings.Exchanges.OrderEvents,
            _settings.RoutingKeys.OrderCreated,
            cancellationToken);

        _logger.LogInformation(
            "Published OrderCreatedIntegrationEvent: EventId={EventId}",
            integrationEvent.EventId);
    }
}