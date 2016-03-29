using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration.Caching
{
    public interface IHaloConfigurationCacheManager
    {
        void Clear(string cacheName = "");

        void Add(string cacheName, string cacheKey, dynamic cacheItem);
        void AddRange(string cacheName, IDictionary<string, dynamic> cacheItems);

        dynamic Find(string cacheName, string cacheKey);
        IEnumerable<dynamic> FindAll(string cacheName);
    }
}
