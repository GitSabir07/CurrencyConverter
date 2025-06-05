using Asp.Versioning;
using ConversionAPI.Application.Interfaces;
using ConversionAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace CourrencyConversionAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/CourrencyConversionAPI")]
    [ApiController]
    public class CourrencyConversionAPIController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyService _currencyService;


        public CourrencyConversionAPIController(ICurrencyService currencyService, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _currencyService = currencyService;
        }

        [HttpGet("latest-rates")]
        [EnableRateLimiting("Fixed")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [NonAction]
        public async Task<ActionResult<ExchangeRate>> GetLatestRatesAsync()
        {

            var response = await _currencyService.GetLatestRatesAsync("USD");

            return Ok(response);
        }

        [HttpGet("latest-rates/{baseCurrency}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<ExchangeRate>> GetLatestRatesByBaseAsync(string baseCurrency)
        {
            if (string.IsNullOrEmpty(baseCurrency))
            {
                return BadRequest("Base currency is required.");
            }
            if (IsUnsupportedCurrency(baseCurrency))
                return BadRequest($"Currency '{baseCurrency}' is not supported.");

         
            var response = await _currencyService.GetLatestRatesAsync(baseCurrency);

            if (response == null)
            {
                return NoContent();
            }

            return Ok(response);
        }

        [HttpGet("historical-rates")]
        [EnableRateLimiting("Fixed")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetHistoricalRatesAsync(
     string baseCurrency, DateTime startDate, DateTime endDate, int page = 1, int pageSize = 10)
        {
            if (IsUnsupportedCurrency(baseCurrency))
                return BadRequest($"Currency '{baseCurrency}' is not supported.");

            string fromDate = string.Empty;
            string toDate = string.Empty;

            if (string.IsNullOrEmpty(baseCurrency))
            {
                return BadRequest("Base currency is required.");
            }

            if (startDate == DateTime.MinValue)
            {
                return BadRequest();
            }
            if (endDate == DateTime.MinValue)
            {
                return BadRequest();
            }
            fromDate = startDate.ToString("yyyy-MM-dd");
            toDate = endDate.ToString("yyyy-MM-dd");

            
            var response = await _currencyService.GetHistoricalRatesAsync(baseCurrency, fromDate, toDate, page, pageSize);

            if(!response.Any())
            {
                return NoContent();
            }

            return Ok(response);
        }

        [HttpGet("convert-currency")]
        [EnableRateLimiting("Fixed")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<ExchangeRate>> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal? amount)
        {
            if (string.IsNullOrEmpty(fromCurrency))
            {
                return BadRequest("from Currency is required.");
            }

            if (string.IsNullOrEmpty(toCurrency))
            {
                return BadRequest("to Currency is required.");
            }
              if (!amount.HasValue)
            {
                return BadRequest("amount is required.");
            }

            if (IsUnsupportedCurrency(fromCurrency))
                return BadRequest($"Currency '{fromCurrency}' is not supported.");

            if (IsUnsupportedCurrency(toCurrency))
                return BadRequest($"Currency '{toCurrency}' is not supported.");

            var amt = amount.Value;
            var response = await _currencyService.ConvertAsync(fromCurrency, toCurrency, amt);
            decimal convertedAmount = amt * response.Rates[toCurrency];

            if (response == null)
            {
                return NoContent();
            }

            return Ok($"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}");

        }

        private bool IsUnsupportedCurrency(string currency)
        {
            return new[] { "TRY", "PLN", "THB", "MXN" }.Contains(currency?.ToUpper());
        }


    }

   

}
