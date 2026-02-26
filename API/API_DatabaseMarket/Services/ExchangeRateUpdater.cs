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
            var rates = await _cbr.GetRatesAsync(ct);

            foreach (var (code, rate) in rates)
            {
                var existing = await _db.ExchangeRates
                    .FirstOrDefaultAsync(x => x.CurrencyCode == code, ct);

                if (existing == null)
                {
                    _db.ExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyCode = code,
                        RateToRub = rate,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.RateToRub = rate;
                    existing.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
