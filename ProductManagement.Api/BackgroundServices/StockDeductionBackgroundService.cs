using ProductManagement.Infrastructure.Messaging.Consumers;

namespace ProductManagement.Api.BackgroundServices;

public class StockDeductionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StockDeductionBackgroundService> _logger;

    public StockDeductionBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<StockDeductionBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stock Deduction Background Service starting");

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wait for app to initialize

        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<StockDeductionConsumer>();

        try
        {
            consumer.StartConsuming();
            _logger.LogInformation("Stock Deduction Consumer started successfully");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Stock Deduction Background Service is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Stock Deduction Background Service");
            throw;
        }
        finally
        {
            consumer.StopConsuming();
            _logger.LogInformation("Stock Deduction Consumer stopped");
        }
    }
}