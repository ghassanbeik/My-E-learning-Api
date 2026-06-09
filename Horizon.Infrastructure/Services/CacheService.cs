

using Horizon.Domain.Interfaces.Services.CacheServices;
using Microsoft.Extensions.Caching.Memory;

namespace Horizon.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly HashSet<string> _keys = new();
        private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);

        public CacheService(IMemoryCache cache) => _cache = cache;

        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default) where T : class
        {
            _keys.Add(key);
            _cache.Set(key, value, expiry ?? DefaultExpiry);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken ct = default)
        {
            _cache.Remove(key);
            _keys.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
        {
            var matching = _keys.Where(k => k.StartsWith(prefix)).ToList();
            foreach (var key in matching)
            {
                _cache.Remove(key);
                _keys.Remove(key);
            }
            return Task.CompletedTask;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken ct = default) where T : class
        {
            var cached = await GetAsync<T>(key, ct);
            if (cached != null) return cached;

            var value = await factory();
            await SetAsync(key, value, expiry, ct);
            return value;
        }

        public Task<bool> ExistsAsync(string key, CancellationToken ct = default)
            => Task.FromResult(_cache.TryGetValue(key, out _));
    }

}
