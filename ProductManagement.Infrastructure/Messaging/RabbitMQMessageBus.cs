using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Messaging;
using RabbitMQ.Client;

namespace ProductManagement.Infrastructure.Messaging;

public class RabbitMQMessageBus : IMessageBus
{
    private readonly RabbitMQConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQMessageBus> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public RabbitMQMessageBus(
        RabbitMQConnectionFactory connectionFactory,
        ILogger<RabbitMQMessageBus> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public Task PublishAsync<T>(
        T message,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            using var channel = _connectionFactory.CreateChannel();

            // Declare exchange (idempotent)
            channel.ExchangeDeclare(
                exchange: exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Serialize message
            var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
            var body = Encoding.UTF8.GetBytes(messageJson);

            // Set message properties
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // Publish message
            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published {MessageType} to exchange '{Exchange}' with routing key '{RoutingKey}'. MessageId: {MessageId}",
                typeof(T).Name,
                exchange,
                routingKey,
                properties.MessageId);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish {MessageType} to exchange '{Exchange}' with routing key '{RoutingKey}'",
                typeof(T).Name,
                exchange,
                routingKey);
            throw;
        }
    }
}