using Serilog;
using Serilog.Formatting.Compact;

namespace CourrencyConversionAPI
{
    public static class ApplicationExtention
    {
        public static void ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    //.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), "log1.txt", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
