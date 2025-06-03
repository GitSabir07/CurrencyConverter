namespace ConversionAPI.Application.Dtos
{
    public class ExchangeRateDto
    {
        public string BaseCurrency { get; set; } = default!;
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public DateTime Date { get; set; }
    }
}
