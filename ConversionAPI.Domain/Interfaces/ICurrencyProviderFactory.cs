using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionAPI.Domain.Interfaces
{
    public interface ICurrencyProviderFactory
    {
        ICurrencyProvider GetProvider(string providerName);
    }
}
