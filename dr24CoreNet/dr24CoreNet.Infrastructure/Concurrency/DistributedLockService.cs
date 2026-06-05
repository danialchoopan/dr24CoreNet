using System.Collections.Concurrent;

namespace dr24CoreNet.Infrastructure.Concurrency;

public class DistributedLockService
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public async Task<bool> AcquireLockAsync(string resourceKey, TimeSpan timeout)
    {
        var semaphore = _locks.GetOrAdd(resourceKey, _ => new SemaphoreSlim(1, 1));
        return await semaphore.WaitAsync(timeout);
    }

    public void ReleaseLock(string resourceKey)
    {
        if (_locks.TryGetValue(resourceKey, out var semaphore))
        {
            semaphore.Release();
        }
    }
}
