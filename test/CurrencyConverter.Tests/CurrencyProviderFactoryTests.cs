using ConversionAPI.Domain.Interfaces;
using ConversionAPI.Infrastructure;
using ConversionAPI.Infrastructure.ExternalAPIs;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CurrencyConverter.Tests
{
    public class CurrencyProviderFactoryTests
    {
        [Fact]
        public void Constructor_WithNullServiceProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CurrencyProviderFactory(null));
        }

        [Fact]
        public void GetProvider_WithNullOrWhitespace_ThrowsArgumentException()
        {
            var factory = new CurrencyProviderFactory(new ServiceCollection().BuildServiceProvider());
            Assert.Throws<ArgumentException>(() => factory.GetProvider(null));
            Assert.Throws<ArgumentException>(() => factory.GetProvider(""));
            Assert.Throws<ArgumentException>(() => factory.GetProvider("   "));
        }

        [Fact]
        public void GetProvider_WithUnsupportedProvider_ThrowsArgumentException()
        {
            var factory = new CurrencyProviderFactory(new ServiceCollection().BuildServiceProvider());
            Assert.Throws<ArgumentException>(() => factory.GetProvider("UnknownProvider"));
        }

        [Fact]
        public void GetProvider_WithFrankfurter_ResolvesProvider()
        {
            var services = new ServiceCollection();
            // Register FrankfurterAPI with a dummy HttpClient
            services.AddSingleton<FrankfurterAPI>(sp => new FrankfurterAPI(new HttpClient()));
            var provider = services.BuildServiceProvider();

            var factory = new CurrencyProviderFactory(provider);
            var result = factory.GetProvider("Frankfurter");
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICurrencyProvider>(result);
        }

        [Fact]
        public void GetProvider_WhenServiceNotRegistered_ThrowsInvalidOperationException()
        {
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var factory = new CurrencyProviderFactory(provider);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.GetProvider("Frankfurter"));
            Assert.Contains("Service provider could not resolve type", ex.Message);
        }
    }
}
