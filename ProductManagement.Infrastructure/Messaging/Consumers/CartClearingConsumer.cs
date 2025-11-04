using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Cart.Commands;
using ProductManagement.Application.IntegrationEvents;

namespace ProductManagement.Infrastructure.Messaging.Consumers;

public class CartClearingConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CartClearingConsumer> _logger;

    public CartClearingConsumer(
        IMediator mediator,
        ILogger<CartClearingConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Processing cart clearing for user. UserId: {UserId}, OrderId: {OrderId}",
            message.UserId,
            message.OrderId);

        // Clear cart command
        var command = new ClearCartCommand(message.UserId);

        // Execute command
        await _mediator.Send(command);

        _logger.LogInformation(
            "Cart cleared successfully. UserId: {UserId}, OrderId: {OrderId}",
            message.UserId,
            message.OrderId);
    }
}
