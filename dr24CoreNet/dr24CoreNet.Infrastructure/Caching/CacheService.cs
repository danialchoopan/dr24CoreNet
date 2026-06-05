using Microsoft.Extensions.Caching.Memory;

namespace dr24CoreNet.Infrastructure.Caching;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private static readonly string DoctorListKey = "DoctorList";
    private static readonly string SpecializationListKey = "SpecializationList";

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? GetOrSet<T>(string key, Func<T> factory, TimeSpan slidingExpiration)
    {
        if (!_cache.TryGetValue(key, out T? value))
        {
            value = factory();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(slidingExpiration);
            _cache.Set(key, value, cacheEntryOptions);
        }
        return value;
    }

    public void InvalidateDoctors()
    {
        _cache.Remove(DoctorListKey);
    }

    public void InvalidateSpecializations()
    {
        _cache.Remove(SpecializationListKey);
    }
}
