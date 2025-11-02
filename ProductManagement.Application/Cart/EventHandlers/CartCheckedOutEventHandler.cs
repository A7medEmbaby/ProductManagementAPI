using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Messaging;
using ProductManagement.Application.Settings;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartCheckedOutEventHandler : INotificationHandler<CartCheckedOutEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<CartCheckedOutEventHandler> _logger;

    public CartCheckedOutEventHandler(
        IMessageBus messageBus,
        IOptions<RabbitMQSettings> settings,
        ILogger<CartCheckedOutEventHandler> logger)
    {
        _messageBus = messageBus;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(CartCheckedOutEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cart checked out: CartId={CartId}, UserId={UserId}, Items={ItemCount}, Total={Total}",
            notification.CartId.Value,
            notification.UserId.Value,
            notification.Items.Count,
            notification.TotalAmount);

        // Map to integration event
        var integrationEvent = new CartCheckedOutIntegrationEvent
        {
            EventId = Guid.NewGuid(),
            CartId = notification.CartId.Value,
            UserId = notification.UserId.Value,
            Items = notification.Items.Select(item => new CartItemDto
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
            _settings.Exchanges.CartEvents,
            _settings.RoutingKeys.CartCheckedOut,
            cancellationToken);

        _logger.LogInformation(
            "Published CartCheckedOutIntegrationEvent: EventId={EventId}",
            integrationEvent.EventId);
    }
}