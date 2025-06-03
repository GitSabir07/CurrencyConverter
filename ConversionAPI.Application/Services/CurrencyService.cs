using ConversionAPI.Application.Interfaces;
using ConversionAPI.Domain.Entities;
using ConversionAPI.Domain.Interfaces;

namespace ConversionAPI.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly ICurrencyProviderFactory _clientFactory;

        // public CurrencyService(ICurrencyProvider currencyProvider, ICurrencyProviderFactory clientFactory)
        public CurrencyService(ICurrencyProviderFactory clientFactory)
        {
            // _currencyProvider = currencyProvider;
            _currencyProvider = clientFactory.GetProvider("Frankfurter");
        }
        public async Task<ExchangeRate> ConvertAsync(string from, string to, decimal amount)
        {
            
            return await _currencyProvider.ConvertAsync(from, to, amount);
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetHistoricalRatesAsync(string baseCurrency, string fromDate, string toDate, int page, int pageSize)
        {
            return await _currencyProvider.GetHistoricalRatesAsync(baseCurrency, fromDate, toDate, page, pageSize);
        }

        public async Task<ExchangeRate> GetLatestRatesAsync(string baseCurrency)
        {
            return await _currencyProvider.GetLatestRatesAsync(baseCurrency);
        }
    }
}
