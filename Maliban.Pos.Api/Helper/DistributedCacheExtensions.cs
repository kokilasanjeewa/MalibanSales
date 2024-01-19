using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Maliban.Pos.Api.Helper
{
    public static class DistributedCacheExtensions
    {
        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions());
        }
        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, GetJsonSerializerOptions()));
            return cache.SetAsync(key, bytes, options);
        }
        public static async Task<T> GetValuesasync<T>(this IDistributedCache cache, string key)
        {
            var jsonData = await cache.GetStringAsync(key);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T value)
        {
            var val = cache.Get(key);
            value = default;

            if (val == null) return false;

            value = JsonSerializer.Deserialize<T>(val, GetJsonSerializerOptions());

            return true;
        }
         private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
