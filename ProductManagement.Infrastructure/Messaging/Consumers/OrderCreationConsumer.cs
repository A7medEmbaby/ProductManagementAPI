using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.IntegrationEvents;
using ProductManagement.Application.Orders.Commands;
using ProductManagement.Domain.Common.ValueObjects;
using ProductManagement.Domain.Orders.Events;
using ProductManagement.Domain.Products.ValueObjects;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class OrderCreationConsumer : IConsumer<CartCheckedOutIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderCreationConsumer> _logger;

    public OrderCreationConsumer(
        IMediator mediator,
        ILogger<OrderCreationConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CartCheckedOutIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Processing cart checkout for order creation. UserId: {UserId}, CartId: {CartId}",
            message.UserId,
            message.CartId);

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
        var result = await _mediator.Send(command);

        _logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}, UserId: {UserId}",
            result.OrderId,
            message.UserId);
    }
}
