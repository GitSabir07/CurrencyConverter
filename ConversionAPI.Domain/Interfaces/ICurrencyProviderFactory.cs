namespace ConversionAPI.Domain.Interfaces
{
    public interface ICurrencyProviderFactory
    {
        ICurrencyProvider GetProvider(string providerName);
    }
}
