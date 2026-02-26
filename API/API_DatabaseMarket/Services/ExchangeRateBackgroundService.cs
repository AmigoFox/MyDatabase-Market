using Microsoft.Extensions.Hosting;

namespace API_DatabaseMarket.Services
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExchangeRateBackgroundService> _logger;

        public ExchangeRateBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ExchangeRateBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var updater = scope.ServiceProvider
                        .GetRequiredService<ExchangeRateUpdater>();

                    await updater.UpdateRatesAsync(stoppingToken);

                    _logger.LogInformation("Exchange rates updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating exchange rates.");
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}