namespace Abp.Runtime.Caching
{
    using System;

    using Abp.Application.Services;

    public interface IThreadSafeCacheFactoryService : IApplicationService
    {
        IThreadSafeCache<TValue> CreateThreadSafeObjectCache<TValue>(string name, TimeSpan slidingExpiration) where TValue : class;
    }
}
