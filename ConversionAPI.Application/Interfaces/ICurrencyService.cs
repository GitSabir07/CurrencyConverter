using ConversionAPI.Application.Dtos;
using ConversionAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionAPI.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<ExchangeRate> GetLatestRatesAsync(string baseCurrency);
        Task<ExchangeRate> ConvertAsync(string from, string to, decimal amount);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetHistoricalRatesAsync(string baseCurrency, string fromDate, string toDate, int page, int pageSize);
    }

}
