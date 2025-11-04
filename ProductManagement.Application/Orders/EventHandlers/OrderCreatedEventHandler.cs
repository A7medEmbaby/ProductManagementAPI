using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Domain.Orders.Events;

namespace ProductManagement.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<OrderCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
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

        // Publish via MassTransit
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation(
            "Published OrderCreatedIntegrationEvent: EventId={EventId}",
            integrationEvent.EventId);
    }
}
