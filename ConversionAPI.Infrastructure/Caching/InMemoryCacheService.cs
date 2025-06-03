using Microsoft.Extensions.Caching.Memory;

namespace ConversionAPI.Infrastructure.Caching
{
    public class InMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheService(IMemoryCache cache) => _cache = cache;

        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }
    }
}
