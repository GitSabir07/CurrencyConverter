using DotNetCrudWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.IntegrationTests
{
    public class CourrencyControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CourrencyControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetLatestRatesByBaseAsync_ReturnsSuccessAndMoviesList()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/CourrencyConversionAPI/GetLatestRatesByBaseAsync/EUR");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var json = await response.Content.ReadAsStringAsync();
            Assert.Contains("title", json, System.StringComparison.OrdinalIgnoreCase); // Adjust "title" to a property your Movie DTO returns
        }


    }
}
