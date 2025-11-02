using ProductManagement.Infrastructure.Messaging.Consumers;

namespace ProductManagement.Api.BackgroundServices;

public class OrderCreationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderCreationBackgroundService> _logger;

    public OrderCreationBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<OrderCreationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order Creation Background Service starting");

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wait for app to initialize

        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<OrderCreationConsumer>();

        try
        {
            consumer.StartConsuming();
            _logger.LogInformation("Order Creation Consumer started successfully");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Order Creation Background Service is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Order Creation Background Service");
            throw;
        }
        finally
        {
            consumer.StopConsuming();
            _logger.LogInformation("Order Creation Consumer stopped");
        }
    }
}