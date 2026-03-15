using CrossApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrossApp.Services.Api
{
    public class OrdersService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly AuthTokenStore _tokenStore;

        public OrdersService(HttpClient httpClient, AuthTokenStore tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public async Task<int?> CreateAsync(CreateOrderRequest request)
        {
            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("orders", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<int>(result, _jsonOptions);
        }

    }
}