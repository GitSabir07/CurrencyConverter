using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DotNetCrudWebApi
{
    public static class ObservabilityConfig
    {
        public static void ConfigureObservability(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("DotNetCrudWebApi")
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Tracing.Net"))
                    .AddConsoleExporter());

            services.AddLogging(logging =>
            {
                logging.ClearProviders()
                    .AddConsole()
                    .AddDebug()
                    .AddOpenTelemetry(opt =>
                    {
                        opt.AddConsoleExporter()
                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CurrencyService.Net"));
                        opt.IncludeFormattedMessage = true;
                        opt.IncludeScopes = true;
                    });
            });
        }
    }
}
