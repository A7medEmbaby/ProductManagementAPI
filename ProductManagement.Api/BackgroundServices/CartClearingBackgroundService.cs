using ProductManagement.Infrastructure.Messaging.Consumers;

namespace ProductManagement.Api.BackgroundServices;

public class CartClearingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CartClearingBackgroundService> _logger;

    public CartClearingBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<CartClearingBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cart Clearing Background Service starting");

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wait for app to initialize

        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<CartClearingConsumer>();

        try
        {
            consumer.StartConsuming();
            _logger.LogInformation("Cart Clearing Consumer started successfully");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Cart Clearing Background Service is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Cart Clearing Background Service");
            throw;
        }
        finally
        {
            consumer.StopConsuming();
            _logger.LogInformation("Cart Clearing Consumer stopped");
        }
    }
}