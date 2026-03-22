using CrossApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CrossApp.Services.Api
{
    public class PaymentsService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly AuthTokenStore _tokenStore;

        public PaymentsService(HttpClient httpClient, AuthTokenStore tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<PaymentDto>> GetPayments()
        {
            var response = await _httpClient.GetAsync("payments");

            if (!response.IsSuccessStatusCode)
                return new List<PaymentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<PaymentDto>>(json, _jsonOptions);
        }

        public async Task CreatePayment(CreatePaymentRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var masked = _tokenStore.GetToken() is string t ? (t.Length > 10 ? t.Substring(0,10)+"..." : t) : "(null)";
            Debug.WriteLine($"PaymentsService.CreatePayment: using token '{masked}'");

            Debug.WriteLine($"PaymentsService.CreatePayment: request json: {json}");
            Debug.WriteLine($"PaymentsService.CreatePayment: OrderId={request?.OrderId}");

            var response = await _httpClient.PostAsync("payments", content);

            var responseText = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"PAYMENT RESPONSE: {response.StatusCode}");
            Debug.WriteLine(responseText);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Payment error: {response.StatusCode} - {responseText}");
            }
        }
    }
}
