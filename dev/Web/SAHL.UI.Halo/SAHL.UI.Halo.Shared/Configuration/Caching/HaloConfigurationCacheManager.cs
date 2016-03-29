using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SAHL.UI.Halo.Shared.Configuration.Caching
{
    public class HaloConfigurationCacheManager : IHaloConfigurationCacheManager
    {
        private object cacheLock = new object();
        private ConcurrentDictionary<string, IList<CacheItem>> itemCache;

        public HaloConfigurationCacheManager()
        {
            this.itemCache = new ConcurrentDictionary<string, IList<CacheItem>>();
        }

        protected ConcurrentDictionary<string, IList<CacheItem>> CacheItems
        {
            get { return this.itemCache; }
        }

        public void Clear(string cacheName = "")
        {
            lock (this.cacheLock)
            {
                if (string.IsNullOrWhiteSpace(cacheName))
                {
                    this.itemCache.Clear();
                }
                else
                {
                    IList<CacheItem> cacheItems;
                    this.itemCache.TryRemove(cacheName, out cacheItems);
                }
            }
        }

        public void Add(string cacheName, string cacheKey, dynamic cacheItem)
        {
            if (string.IsNullOrWhiteSpace(cacheName)) { throw  new ArgumentNullException("cacheName"); }
            if (string.IsNullOrWhiteSpace(cacheKey)) { throw  new ArgumentNullException("cacheKey"); }
            if (cacheItem == null ) { throw  new ArgumentNullException("cacheItem"); }

            this.EnsureCacheNameExistsInCache(cacheName);

            var existingCacheItem = this.Find(cacheName, cacheKey);

            lock (this.cacheLock)
            {
                if (existingCacheItem == null)
                {
                    this.itemCache[cacheName].Add(new CacheItem(cacheKey, cacheItem));
                }
            }
        }

        public void AddRange(string cacheName, IDictionary<string, dynamic> cacheItems)
        {
            if (string.IsNullOrWhiteSpace(cacheName)) { throw new ArgumentNullException("cacheName"); }
            if (cacheItems == null || !cacheItems.Any()) { throw new ArgumentNullException("cacheItems"); }

            foreach (var cacheItem in cacheItems)
            {
                this.Add(cacheName, cacheItem.Key, cacheItem.Value);
            }
        }

        public dynamic Find(string cacheName, string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheName)) { throw new ArgumentNullException("cacheName"); }
            if (string.IsNullOrWhiteSpace(cacheKey)) { throw new ArgumentNullException("cacheKey"); }

            lock (this.cacheLock)
            {
                IList<CacheItem> cacheItems;
                if (!this.itemCache.TryGetValue(cacheName, out cacheItems))
                {
                    return null;
                }

                var foundItem = cacheItems.FirstOrDefault(item => item.Key == cacheKey);
                return foundItem == null
                    ? null
                    : foundItem.Item;
            }
        }

        public IEnumerable<dynamic> FindAll(string cacheName)
        {
            if (string.IsNullOrWhiteSpace(cacheName)) { throw new ArgumentNullException("cacheName"); }

            IList<CacheItem> cacheItems;
            lock (this.cacheLock)
            {
                if (!this.itemCache.TryGetValue(cacheName, out cacheItems))
                {
                    return null;
                }
            }

            return cacheItems.Select(item => item.Item);
        }

        private void EnsureCacheNameExistsInCache(string cacheName)
        {
            lock (this.cacheLock)
            {
                if (this.itemCache.ContainsKey(cacheName)) { return; }

                var caheItems = new List<CacheItem>();
                this.itemCache.TryAdd(cacheName, caheItems);
            }
        }
    }
}
