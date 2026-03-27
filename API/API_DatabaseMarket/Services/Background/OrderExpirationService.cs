using API_DatabaseMarket.Data;
using Microsoft.EntityFrameworkCore;

namespace API_DatabaseMarket.Services.Background;

public class OrderExpirationService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<OrderExpirationService> _logger;

    public OrderExpirationService(IServiceProvider services, ILogger<OrderExpirationService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderExpirationService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var updated = await db.Orders
                    .Where(o => o.PaymentDueDate < DateTime.UtcNow && o.Status != "paid")
                    .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, "expired"));

                _logger.LogInformation($"Expired orders updated: {updated}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating expired orders");
            }

            await Task.Delay(TimeSpan.FromMinutes(35), stoppingToken);
        }
    }
}