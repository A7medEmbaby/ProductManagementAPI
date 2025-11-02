using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProductManagement.Infrastructure.Messaging;

public abstract class RabbitMQConsumerBase<T> : IDisposable where T : class
{
    private readonly RabbitMQConnectionFactory _connectionFactory;
    private readonly RabbitMQSettings _settings;
    protected readonly ILogger Logger;
    private readonly JsonSerializerOptions _jsonOptions;

    private IModel? _channel;
    private string? _consumerTag;
    private bool _disposed;

    protected abstract string QueueName { get; }
    protected abstract string ExchangeName { get; }
    protected abstract string RoutingKey { get; }

    protected RabbitMQConsumerBase(
        RabbitMQConnectionFactory connectionFactory,
        IOptions<RabbitMQSettings> settings,
        ILogger logger)
    {
        _connectionFactory = connectionFactory;
        _settings = settings.Value;
        Logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public void StartConsuming()
    {
        try
        {
            _channel = _connectionFactory.CreateChannel();

            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Declare queue
            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);

            // Set QoS
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            // Create consumer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                await OnMessageReceivedAsync(ea);
            };

            _consumerTag = _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);

            Logger.LogInformation(
                "Started consuming from queue '{QueueName}' bound to exchange '{ExchangeName}' with routing key '{RoutingKey}'",
                QueueName,
                ExchangeName,
                RoutingKey);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to start consuming from queue '{QueueName}'", QueueName);
            throw;
        }
    }

    private async Task OnMessageReceivedAsync(BasicDeliverEventArgs ea)
    {
        var messageId = ea.BasicProperties?.MessageId ?? "unknown";

        try
        {
            Logger.LogInformation(
                "Received message from queue '{QueueName}'. MessageId: {MessageId}",
                QueueName,
                messageId);

            // Deserialize message
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(messageJson, _jsonOptions);

            if (message == null)
            {
                Logger.LogWarning("Failed to deserialize message. MessageId: {MessageId}", messageId);
                _channel?.BasicNack(ea.DeliveryTag, false, false);
                return;
            }

            // Process message with retry logic
            var processed = await ProcessWithRetryAsync(message, messageId);

            if (processed)
            {
                _channel?.BasicAck(ea.DeliveryTag, false);
                Logger.LogInformation(
                    "Successfully processed message from queue '{QueueName}'. MessageId: {MessageId}",
                    QueueName,
                    messageId);
            }
            else
            {
                // Reject and don't requeue (move to DLQ if configured)
                _channel?.BasicNack(ea.DeliveryTag, false, false);
                Logger.LogError(
                    "Failed to process message after retries from queue '{QueueName}'. MessageId: {MessageId}",
                    QueueName,
                    messageId);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(
                ex,
                "Unexpected error processing message from queue '{QueueName}'. MessageId: {MessageId}",
                QueueName,
                messageId);

            _channel?.BasicNack(ea.DeliveryTag, false, false);
        }
    }

    private async Task<bool> ProcessWithRetryAsync(T message, string messageId)
    {
        const int maxRetries = 3;
        var delay = TimeSpan.FromSeconds(1);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Logger.LogDebug(
                    "Processing message (attempt {Attempt}/{MaxRetries}). MessageId: {MessageId}",
                    attempt,
                    maxRetries,
                    messageId);

                await ProcessMessageAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(
                    ex,
                    "Failed to process message (attempt {Attempt}/{MaxRetries}). MessageId: {MessageId}",
                    attempt,
                    maxRetries,
                    messageId);

                if (attempt < maxRetries)
                {
                    await Task.Delay(delay);
                    delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2); // Exponential backoff
                }
            }
        }

        return false;
    }

    protected abstract Task ProcessMessageAsync(T message);

    public void StopConsuming()
    {
        try
        {
            if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
            {
                _channel.BasicCancel(_consumerTag);
                Logger.LogInformation("Stopped consuming from queue '{QueueName}'", QueueName);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while stopping consumer for queue '{QueueName}'", QueueName);
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        StopConsuming();

        try
        {
            _channel?.Close();
            _channel?.Dispose();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while disposing consumer for queue '{QueueName}'", QueueName);
        }
    }
}