using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Domain.Cart.Events;

namespace ProductManagement.Application.Cart.EventHandlers;

public class CartCheckedOutEventHandler : INotificationHandler<CartCheckedOutEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CartCheckedOutEventHandler> _logger;

    public CartCheckedOutEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<CartCheckedOutEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
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

        // Publish via MassTransit
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "Published CartCheckedOutIntegrationEvent: EventId={EventId}",
            integrationEvent.EventId);
    }
}
