using API_DatabaseMarket.Data;
using API_DatabaseMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace API_DatabaseMarket.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly AppDbContext _db;
        public ExchangeRateService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> ConvertFromRubAsync(decimal rub, string currencyCode)
        {
            var rate = await _db.ExchangeRates
                .FirstOrDefaultAsync(x => x.CurrencyCode == currencyCode);

            if (rate == null)
                throw new Exception("Exchange rate not found");

            return Math.Round(rub / rate.RateToRub, 2);
        }
    }
}
