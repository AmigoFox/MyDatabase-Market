using API_DatabaseMarket.Models;
using API_DatabaseMarket.Data;
using Microsoft.EntityFrameworkCore;
using API_DatabaseMarket.Services;

namespace API_DatabaseMarket.Services
{
    public class ExchangeRateUpdater
    {
        private readonly AppDbContext _db;
        private readonly ICbrExchangeRateService _cbr;
        public ExchangeRateUpdater(AppDbContext db, ICbrExchangeRateService cbr)
        {
            _db = db;
            _cbr = cbr;
        }
        public async Task UpdateRatesAsync(CancellationToken ct = default)
        {
            var ratesFromCbr = await _cbr.GetRatesAsync(ct);

            var existingRates = await _db.ExchangeRates
                .ToDictionaryAsync(x => x.CurrencyCode, ct);

            foreach (var (code, rate) in ratesFromCbr)
            {
                if (existingRates.TryGetValue(code, out var existing))
                {
                    existing.RateToRub = rate;
                    existing.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    _db.ExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyCode = code,
                        RateToRub = rate,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
