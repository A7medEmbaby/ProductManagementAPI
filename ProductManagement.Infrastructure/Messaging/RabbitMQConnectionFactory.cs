using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Settings;
using RabbitMQ.Client;

namespace ProductManagement.Infrastructure.Messaging;

public class RabbitMQConnectionFactory : IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQConnectionFactory> _logger;
    private IConnection? _connection;
    private readonly object _lock = new();
    private bool _disposed;

    public RabbitMQConnectionFactory(
        IOptions<RabbitMQSettings> settings,
        ILogger<RabbitMQConnectionFactory> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public IConnection GetConnection()
    {
        if (_connection != null && _connection.IsOpen)
        {
            return _connection;
        }

        lock (_lock)
        {
            if (_connection != null && _connection.IsOpen)
            {
                return _connection;
            }

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                _connection = factory.CreateConnection();

                _logger.LogInformation(
                    "RabbitMQ connection established to {HostName}:{Port}",
                    _settings.HostName,
                    _settings.Port);

                return _connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create RabbitMQ connection");
                throw;
            }
        }
    }

    public IModel CreateChannel()
    {
        var connection = GetConnection();
        var channel = connection.CreateModel();

        _logger.LogDebug("RabbitMQ channel created");

        return channel;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        try
        {
            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
                _logger.LogInformation("RabbitMQ connection closed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while closing RabbitMQ connection");
        }
    }
}