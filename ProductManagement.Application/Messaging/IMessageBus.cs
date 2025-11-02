namespace ProductManagement.Application.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(
        T message,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken = default) where T : class;
}