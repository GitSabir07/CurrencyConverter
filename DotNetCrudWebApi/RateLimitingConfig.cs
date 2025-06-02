using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace DotNetCrudWebApi
{
    public static class RateLimitingConfig
    {
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("Fixed", opt =>
                {
                    opt.PermitLimit = 100;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 2;
                });
            });
        }
    }
}
