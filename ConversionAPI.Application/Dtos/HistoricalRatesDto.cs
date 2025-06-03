namespace ConversionAPI.Application.Dtos
{
    public class HistoricalRatesDto
    {
        public string Base { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
        public string Date { get; set; }
    }
}
