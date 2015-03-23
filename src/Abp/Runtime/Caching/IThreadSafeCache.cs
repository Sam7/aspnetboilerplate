// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IThreadSafeObjectCache.cs" company="">
//   
// </copyright>
// <summary>
//   Interface for any thread safe object cache
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Abp.Runtime.Caching
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Interface for any thread safe object cache
    /// </summary>
    public interface IThreadSafeCache<TValue> where TValue : class
    {
        /// <summary>
        /// Gets an item from cache if exists, or null.
        /// </summary>
        /// <param name="key">Key to get item</param>
        TValue Get(string key);

        /// <summary>
        /// Gets an item from cache if exists, or calls <paramref name="factoryMethod"/> to create cache item and return it.
        /// </summary>
        /// <param name="key">Key to get item</param>
        /// <param name="factoryMethod">A factory method to create item if it's not exists in cache</param>
        TValue Get(string key, Func<TValue> factoryMethod);

        /// <summary>
        /// Gets an item from cache if exists, or calls <paramref name="factoryMethod"/> to create cache item and return it.
        /// </summary>
        /// <param name="key">Key to get item</param>
        /// <param name="slidingExpiration">Sliding expiration policy</param>
        /// <param name="factoryMethod">A factory method to create item if it's not exists in cache</param>
        TValue Get(string key, TimeSpan slidingExpiration, Func<TValue> factoryMethod);

        /// <summary>
        /// Gets an item from cache if exists, or calls <paramref name="factoryMethod"/> to create cache item and return it.
        /// </summary>
        /// <param name="key">Key to get item</param>
        /// <param name="absoluteExpiration">Absolute expiration policy</param>
        /// <param name="factoryMethod">A factory method to create item if it's not exists in cache</param>
        TValue Get(string key, DateTimeOffset absoluteExpiration, Func<TValue> factoryMethod);

        /// <summary>
        /// Gets an item from cache if exists, or calls <paramref name="factoryMethod"/> to create cache item and return it.
        /// </summary>
        /// <param name="key">Key to get item</param>
        /// <param name="cacheItemPolicy">Cache policy creation method (called only if item is being added to the cache)</param>
        /// <param name="factoryMethod">A factory method to create item if it's not exists in cache</param>
        TValue Get(string key, Func<CacheItemPolicy> cacheItemPolicy, Func<TValue> factoryMethod);

        /// <summary>
        /// Adds/replaces an item in the cache.
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value of the item</param>
        void Set(string key, TValue value);

        /// <summary>
        /// Adds/replaces an item in the cache.
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value of the item</param>
        /// <param name="slidingExpiration">Sliding expiration policy</param>
        void Set(string key, TValue value, TimeSpan slidingExpiration);

        /// <summary>
        /// Adds/replaces an item in the cache.
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value of the item</param>
        /// <param name="absoluteExpiration">Absolute expiration policy</param>
        void Set(string key, TValue value, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Adds/replaces an item in the cache.
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value of the item</param>
        /// <param name="cacheItemPolicy">Cache item policy</param>
        void Set(string key, TValue value, CacheItemPolicy cacheItemPolicy);

        /// <summary>
        /// Removes an item from the cache (if it exists).
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <returns>Removed item (if it exists)</returns>
        TValue Remove(string key);
    }
}
