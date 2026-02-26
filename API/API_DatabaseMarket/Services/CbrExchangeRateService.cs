using System.Text.Json;

namespace API_DatabaseMarket.Services
{
    public interface ICbrExchangeRateService
    {
        Task<Dictionary<string, decimal>> GetRatesAsync(CancellationToken ct = default);
    }

    public class CbrExchangeRateService : ICbrExchangeRateService
    {
        private const string Url = "https://www.cbr-xml-daily.ru/daily_json.js";
        private readonly HttpClient _http;

        public CbrExchangeRateService(HttpClient http)
        {
            _http = http;
            _http.Timeout = TimeSpan.FromSeconds(8);
        }

        public async Task<Dictionary<string, decimal>> GetRatesAsync(CancellationToken ct = default)
        {
            using var response = await _http.GetAsync(Url, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("Valute", out var root))
                throw new InvalidOperationException("Unexpected CBR response structure.");

            var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in root.EnumerateObject())
            {
                var code = kv.Name;
                var valute = kv.Value;

                if (!valute.TryGetProperty("Nominal", out var nominalEl) ||
                    !valute.TryGetProperty("Value", out var valueEl))
                    continue;

                var nominal = nominalEl.GetInt32();
                var value = valueEl.GetDecimal();

                if (nominal > 0)
                    result[code] = Math.Round(value / nominal, 6);
            }

            return result;
        }
    }
}