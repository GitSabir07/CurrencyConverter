using ConversionAPI.Application.Services;
using ConversionAPI.Domain.Entities;
using ConversionAPI.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Tests.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyProviderFactory> _factoryMock;
        private readonly Mock<ICurrencyProvider> _providerMock;
        private readonly CurrencyService _service;

        public CurrencyServiceTests()
        {
            _factoryMock = new Mock<ICurrencyProviderFactory>();
            _providerMock = new Mock<ICurrencyProvider>();
            _factoryMock.Setup(f => f.GetProvider(It.IsAny<string>())).Returns(_providerMock.Object);
            _service = new CurrencyService(_factoryMock.Object);
        }

        [Fact]
        public async Task GetLatestRatesAsync_ReturnsRates()
        {
            _providerMock.Setup(p => p.GetLatestRatesAsync("EUR"))
                .ReturnsAsync(new ExchangeRate { BaseCurrency = "EUR", Date = DateTime.Today, Rates = new Dictionary<string, decimal> { ["USD"] = 1.1m } });

            var result = await _service.GetLatestRatesAsync("EUR");

            Assert.Equal("EUR", result.BaseCurrency);
            Assert.Contains("USD", result.Rates.Keys);
        }

        [Fact]
        public async Task ConvertAsync_ValidCurrency_ReturnsConverted()            
        {
            var ff = Convert.ToDecimal(11.34);
            _providerMock.Setup(p => p.ConvertAsync("USD", "EUR", 100)).ReturnsAsync(new ExchangeRate());
            var result = await _service.ConvertAsync("USD", "EUR", 100);
            Assert.Equal(new ExchangeRate().Rates, result.Rates);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_ReturnsPagedRates()
        {
            // Mocked return data for currency conversion
            var mockData = new Dictionary<string, Dictionary<string, decimal>>
            {
                    { "2024-04-01", new Dictionary<string, decimal> { ["USD"] = 1.1m } },
                    { "2024-04-02", new Dictionary<string, decimal> { ["USD"] = 1.12m } }
            };

            _providerMock.Setup(p => p.GetHistoricalRatesAsync("EUR", It.IsAny<string>(), It.IsAny<string>(), 1, 10))
                .ReturnsAsync(mockData);

            var results = await _service.GetHistoricalRatesAsync("EUR", DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd"),
                DateTime.Today.ToString("yyyy-MM-dd"), 1, 10);

            // Ensure the result is a dictionary with correct data
            Assert.Equal(2, results.Count);
            Assert.Contains(results, kvp => kvp.Key == "2024-04-01" && kvp.Value["USD"] == 1.1m);
            Assert.Contains(results, kvp => kvp.Key == "2024-04-02" && kvp.Value["USD"] == 1.12m);
        }






    }
}
