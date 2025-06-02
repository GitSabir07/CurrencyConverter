using DotNetCrudWebApi.Middlewares;

namespace DotNetCrudWebApi
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseForwardedHeaders();

            // Exception Handling Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Logging Middleware
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await next.Invoke();
                watch.Stop();
                var ip = context.Connection.RemoteIpAddress?.ToString();
                var user = context.User?.Identity?.Name ?? "anonymous";

                logger.LogInformation("{Method} {Path} responded {StatusCode} in {Elapsed} ms from {IP} (User: {User})",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds,
                    ip,
                    user);
            });
        }
    }
}
