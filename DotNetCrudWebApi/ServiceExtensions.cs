using ConversionAPI.Application.Interfaces;
using ConversionAPI.Application.Services;
using ConversionAPI.Domain.Interfaces;
using ConversionAPI.Infrastructure.ExternalAPIs;
using ConversionAPI.Infrastructure;
using DotNetCrudWebApi.Data;

namespace DotNetCrudWebApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database & Memory Cache
            services.AddDbContext<AppDbContext>();
            services.AddMemoryCache();

            // Authentication & Authorization
            services.ConfigureAuthentication(configuration);
            services.ConfigureAuthorization();

            // Swagger Documentation
            services.ConfigureSwagger();

            // API Versioning
            services.ConfigureApiVersioning();

            // OpenTelemetry
            services.ConfigureObservability();

            // Rate Limiting
            services.ConfigureRateLimiting();


            services.AddHttpClient("Frankfurter", client =>
            {
                var baseUrl = configuration["FrankfurterApi:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
            });
            services.AddHttpClient("Frankfurter")
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(PolicyFactoryExtension.GetRetryPolicy())
                .AddPolicyHandler(PolicyFactoryExtension.GetCircuitBreakerPolicy());

            // Register Business Logic & External APIs

            services.AddScoped<ICurrencyProvider, FrankfurterAPI>();
            services.AddScoped<FrankfurterAPI>();
            services.AddScoped<ICurrencyProviderFactory, CurrencyProviderFactory>();
            services.AddScoped<ICurrencyService, CurrencyService>();

        }
    }
}
