using System;

namespace Abp.Runtime.Caching
{
    using System.Runtime.Caching;

    public class ObjectThreadSafeCacheFactoryService : IThreadSafeCacheFactoryService
    {
        public IThreadSafeCache<TValue> CreateThreadSafeObjectCache<TValue>(string name, TimeSpan slidingExpiration) where TValue : class
        {
            return new ThreadSafeObjectCache<TValue>(new MemoryCache(name), slidingExpiration);
        }
    }
}
