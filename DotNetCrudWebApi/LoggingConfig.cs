using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog;

namespace DotNetCrudWebApi
{
    public static class LoggingConfig
    {
        public static void ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.File(new CompactJsonFormatter(), "log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
