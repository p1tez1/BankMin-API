using BankMin_API.Services.IServices;
using System.Text.Json;

namespace BankMin_API.Infrastructure.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private class ExchangeRateResponse
        {
            public string Result { get; set; }
            public ConversionRates Conversion_rates { get; set; }
        }

        private class ConversionRates
        {
            public decimal UAH { get; set; }
        }

        public async Task<decimal> GetUsdToUahRateAsync()
        {
            var response = await _httpClient.GetStringAsync("https://v6.exchangerate-api.com/v6/abbefef4e107bcde6dc96b48/latest/USD");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(response, options);

            if (data == null || data.Conversion_rates == null)
                throw new HttpRequestException("Failed to get currency rates");

            return data.Conversion_rates.UAH;
        }
    }
}
