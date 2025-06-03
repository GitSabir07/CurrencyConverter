using ConversionAPI.Domain.Interfaces;
using ConversionAPI.Infrastructure.ExternalAPIs;

namespace ConversionAPI.Infrastructure
{
    public class CurrencyProviderFactory : ICurrencyProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CurrencyProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ICurrencyProvider GetProvider(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentException("Provider name cannot be null or empty", nameof(providerName));
            }

            return providerName switch
            {
                "Frankfurter" => ResolveProvider<FrankfurterAPI>(),
                _ => throw new ArgumentException($"Unsupported provider: {providerName}", nameof(providerName))
            };
        }

        private ICurrencyProvider ResolveProvider<T>() where T : class, ICurrencyProvider
        {
            var provider = _serviceProvider.GetService(typeof(T)) as T;
            if (provider == null)
            {
                throw new InvalidOperationException($"Service provider could not resolve type {typeof(T).Name}");
            }
            return provider;
        }
    }
}
