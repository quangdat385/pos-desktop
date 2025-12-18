using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PosApi.Interfaces;

namespace PosApi.Infrastructure.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public MemoryCacheService(IMemoryCache memory)
        {
            _memory = memory;
        }

        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            if (_memory.TryGetValue(key, out var obj) && obj is T t)
            {
                return Task.FromResult<T?>(t);
            }
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken ct = default)
        {
            var opts = new MemoryCacheEntryOptions();
            if (absoluteExpirationRelativeToNow.HasValue)
            {
                opts.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            }
            _memory.Set(key, value, opts);
            return Task.CompletedTask;
        }

        // Implement interface signature returning bool
        public Task<bool> SetResponseAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken ct = default)
        {
            var opts = new MemoryCacheEntryOptions();
            if (absoluteExpirationRelativeToNow.HasValue)
            {
                opts.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            }
            _memory.Set(key, value, opts);
            return Task.FromResult(true);
        }

        public Task RemoveAsync(string key, CancellationToken ct = default)
        {
            _memory.Remove(key);
            return Task.CompletedTask;
        }
    }
}
