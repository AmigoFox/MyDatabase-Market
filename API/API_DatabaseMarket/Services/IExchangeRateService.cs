namespace API_DatabaseMarket.Services
{
    public interface IExchangeRateService
    {
        Task<decimal> ConvertFromRubAsync(decimal rub, string currencyCode);
    }
}
