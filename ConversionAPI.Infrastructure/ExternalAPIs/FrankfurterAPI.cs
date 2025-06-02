using ConversionAPI.Application.Dtos;
using ConversionAPI.Domain.Entities;
using ConversionAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConversionAPI.Infrastructure.ExternalAPIs
{
    public class FrankfurterAPI : ICurrencyProvider
    {
        private readonly HttpClient _httpClient;

        public FrankfurterAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ExchangeRate> ConvertAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var url = $"https://api.frankfurter.dev/v1/latest?base={fromCurrency}&symbols={toCurrency}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRate>(url);

            if (response != null && response.Rates.ContainsKey(toCurrency))
            {
                decimal convertedAmount = amount * response.Rates[toCurrency];
                Console.WriteLine($"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}");
            }
            return response ?? new ExchangeRate();
        }

        public async Task<Dictionary<string,Dictionary<string,decimal>>> GetHistoricalRatesAsync(string baseCurrency, string fromDate, string toDate, int page, int pageSize)
        {
            var url = $"https://api.frankfurter.dev/v1/{fromDate}..{toDate}?base={baseCurrency}";

            var response = await _httpClient.GetFromJsonAsync<HistoricalRatesDto>(url);
            if (response == null || response.Rates.Count == 0)
            {
                return new Dictionary<string, Dictionary<string, decimal>>(); // Return empty dictionary if no rates found
                //return NotFound($"No historical rates found for {baseCurrency} between {fromDate} and {toDate}.");
            }

            // Apply Pagination
            var pagedRates = response.Rates.Skip((page - 1) * pageSize).Take(pageSize)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return pagedRates;
        }

        public async Task<ExchangeRate> GetLatestRatesAsync(string baseCurrency)
        {           
            var url = $"https://api.frankfurter.dev/v1/latest?base={baseCurrency}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRate>(url);
            if (response != null)
            {
                Console.WriteLine($"Date: {response.Date}");
                foreach (var rate in response.Rates)
                {
                    Console.WriteLine($"{rate.Key}: {rate.Value}");
                }
            }
            return response ?? new ExchangeRate();
        }
    }
}
