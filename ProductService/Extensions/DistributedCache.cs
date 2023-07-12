using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ProductService.Extensions
{
    public static class DistributedCache
    {
        public static async Task SetRecord<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absExpTime = null, TimeSpan? slidingExpTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absExpTime ?? TimeSpan.FromMinutes(2);
            options.SlidingExpiration = slidingExpTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData);
        }

        public static async Task<T> GetRecord<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if (jsonData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
