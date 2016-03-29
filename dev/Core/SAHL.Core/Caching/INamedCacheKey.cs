using System;

namespace SAHL.Core.Caching
{
    public interface INamedCacheKey
    {
        Type CacheItemType { get; }

        string Name { get; }

        string Context { get; }
    }
}