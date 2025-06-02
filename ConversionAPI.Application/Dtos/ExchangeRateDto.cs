using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionAPI.Application.Dtos
{
    public class ExchangeRateDto
    {
        public string BaseCurrency { get; set; } = default!;
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public DateTime Date { get; set; }
    }
}
