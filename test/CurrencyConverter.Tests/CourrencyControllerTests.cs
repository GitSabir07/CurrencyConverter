using ConversionAPI.Application.Interfaces;
using ConversionAPI.Domain.Entities;
using DotNetCrudWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CurrencyConverter.Tests
{
    public class CourrencyControllerTests
    {
        private readonly Mock<ICurrencyService> _currencyServiceMock;
        private readonly CourrencyConversionAPIController _controller;

        public CourrencyControllerTests()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
            _controller = new CourrencyConversionAPIController(_currencyServiceMock.Object, new HttpClient());
        }

        [Fact]
        public async Task GetLatestRatesAsync_ReturnsOkResult_WithExchangeRate()
        {
            var expected = new ExchangeRate { BaseCurrency = "USD", Rates = new Dictionary<string, decimal> { ["EUR"] = 1.1m } };
            _currencyServiceMock.Setup(s => s.GetLatestRatesAsync("USD")).ReturnsAsync(expected);

            var result = await _controller.GetLatestRatesAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task GetLatestRatesByBaseAsync_UnsupportedCurrency_ReturnsBadRequest()
        {
            var result = await _controller.GetLatestRatesByBaseAsync("TRY");

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("not supported", badRequest.Value.ToString());
        }

        [Fact]
        public async Task GetLatestRatesByBaseAsync_SupportedCurrency_ReturnsOk()
        {
            var expected = new ExchangeRate { BaseCurrency = "EUR", Rates = new Dictionary<string, decimal> { ["USD"] = 1.1m } };
            _currencyServiceMock.Setup(s => s.GetLatestRatesAsync("EUR")).ReturnsAsync(expected);

            var result = await _controller.GetLatestRatesByBaseAsync("EUR");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_UnsupportedCurrency_ReturnsBadRequest()
        {
            var result = await _controller.GetHistoricalRatesAsync("PLN", DateTime.Today, DateTime.Today, 1, 10);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("not supported", badRequest.Value?.ToString());
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_InvalidStartDate_ReturnsBadRequest()
        {
            var result = await _controller.GetHistoricalRatesAsync("USD", DateTime.MinValue, DateTime.Today, 1, 10);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_InvalidEndDate_ReturnsBadRequest()
        {
            var result = await _controller.GetHistoricalRatesAsync("USD", DateTime.Today, DateTime.MinValue, 1, 10);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetHistoricalRatesAsync_ValidRequest_ReturnsOk()
        {
            var expected = new Dictionary<string, Dictionary<string, decimal>>
            {
                { "2024-05-01", new Dictionary<string, decimal> { ["USD"] = 1.1m } }
            };
            _currencyServiceMock.Setup(s => s.GetHistoricalRatesAsync("USD", It.IsAny<string>(), It.IsAny<string>(), 1, 10))
                .ReturnsAsync(expected);

            var result = await _controller.GetHistoricalRatesAsync("USD", DateTime.Today.AddDays(-1), DateTime.Today, 1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task ConvertCurrencyAsync_UnsupportedFromCurrency_ReturnsBadRequest()
        {
            var result = await _controller.ConvertCurrencyAsync("PLN", "USD", 100);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("not supported", badRequest.Value.ToString());
        }

        [Fact]
        public async Task ConvertCurrencyAsync_UnsupportedToCurrency_ReturnsBadRequest()
        {
            var result = await _controller.ConvertCurrencyAsync("USD", "THB", 100);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("not supported", badRequest.Value.ToString());
        }

        [Fact]
        public async Task ConvertCurrencyAsync_ValidRequest_ReturnsOk()
        {
            var rates = new Dictionary<string, decimal> { ["EUR"] = 0.9m };
            var exchangeRate = new ExchangeRate { BaseCurrency = "USD", Rates = rates };
            _currencyServiceMock.Setup(s => s.ConvertAsync("USD", "EUR", 100)).ReturnsAsync(exchangeRate);

            var result = await _controller.ConvertCurrencyAsync("USD", "EUR", 100);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("100 USD = 90.00 EUR", okResult.Value);
        }
    }
}