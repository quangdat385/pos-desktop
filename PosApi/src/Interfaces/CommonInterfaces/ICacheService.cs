using System;
using System.Threading;
using System.Threading.Tasks;

namespace PosApi.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken ct = default);
        Task<bool> SetResponseAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken ct = default);
        Task RemoveAsync(string key, CancellationToken ct = default);
    }
}
