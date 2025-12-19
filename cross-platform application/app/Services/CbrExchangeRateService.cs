using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace app.Services;

public class CbrExchangeRateService
{
    private const string Url = "https://www.cbr-xml-daily.ru/daily_json.js";
    private readonly HttpClient _http;

    public CbrExchangeRateService(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _http.Timeout = TimeSpan.FromSeconds(8);
    }

    public async Task<Dictionary<string, decimal>> GetRatesAsync(CancellationToken ct = default)
    {
        var response = await _http.GetAsync(Url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("Valute", out var root))
            throw new InvalidOperationException("Unexpected CBR response structure: missing 'Valute'.");

        var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        foreach (var kv in root.EnumerateObject())
        {
            var code = kv.Name;
            var valute = kv.Value;

            if (!valute.TryGetProperty("Nominal", out var nominalEl) ||
                !valute.TryGetProperty("Value", out var valueEl))
            {
                continue;
            }

            var nominal = nominalEl.GetInt32();
            var value = valueEl.GetDecimal();

            if (nominal > 0)
                result[code] = Math.Round(value / nominal, 4);
        }

        if (result.Count == 0)
            throw new InvalidOperationException("No exchange rates were parsed from CBR response.");

        return result;
    }
}