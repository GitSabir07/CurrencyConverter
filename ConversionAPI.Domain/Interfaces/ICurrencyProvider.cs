using ConversionAPI.Domain.Entities;

namespace ConversionAPI.Domain.Interfaces
{
    public interface ICurrencyProvider
    {
        Task<ExchangeRate?> GetLatestRatesAsync(string baseCurrency);
        Task<ExchangeRate?> ConvertAsync(string from, string to, decimal amount);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetHistoricalRatesAsync(string baseCurrency, string fromDate, string toDate, int page, int pageSize);
    }

}
