using System;

namespace SAHL.Core.Caching
{
    public interface IKeyedCacheKey
    {
        Type CacheItemType { get; }

        int Key { get; }

        string Context { get; }
    }
}