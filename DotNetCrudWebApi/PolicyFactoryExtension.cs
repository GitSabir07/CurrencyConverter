using Polly.Extensions.Http;
using Polly;

namespace CourrencyConversionAPI
{
    public static class PolicyFactoryExtension
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
            {
                var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100));
                return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + jitter;
            });

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) => { },
                    onReset: () => { },
                    onHalfOpen: () => { });
    }
}

