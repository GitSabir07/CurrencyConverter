using Asp.Versioning;
using ConversionAPI.Application.Interfaces;
using ConversionAPI.Domain.Entities;
using DotNetCrudWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DotNetCrudWebApi.Controllers
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
            if (IsUnsupportedCurrency(baseCurrency))
                return BadRequest($"Currency '{baseCurrency}' is not supported.");

         
            var response = await _currencyService.GetLatestRatesAsync(baseCurrency);

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

            return Ok(response);
        }

        [HttpGet("convert-currency")]
        [EnableRateLimiting("Fixed")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult<ExchangeRate>> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            if (IsUnsupportedCurrency(fromCurrency))
                return BadRequest($"Currency '{fromCurrency}' is not supported.");

            if (IsUnsupportedCurrency(toCurrency))
                return BadRequest($"Currency '{toCurrency}' is not supported.");


           var response = await _currencyService.ConvertAsync(fromCurrency, toCurrency, amount);
            decimal convertedAmount = amount * response.Rates[toCurrency];
            return Ok($"{amount} {fromCurrency} = {convertedAmount:F2} {toCurrency}");

        }

        private bool IsUnsupportedCurrency(string currency)
        {
            return new[] { "TRY", "PLN", "THB", "MXN" }.Contains(currency?.ToUpper());
        }


    }

    //public class ExchangeRates
    //{
    //    public string Base { get; set; }
    //    public string Date { get; set; }
    //    public Dictionary<string, decimal> Rates { get; set; }
    //}

    //public class HistoricalRates
    //{
    //    public string Base { get; set; }
    //    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    //    public string Date { get; set; }
    //}

}
