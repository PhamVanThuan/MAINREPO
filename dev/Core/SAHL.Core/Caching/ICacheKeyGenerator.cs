namespace SAHL.Core.Caching
{
    public interface ICacheKeyGenerator
    {
        string GetKey<CacheKeyType, CacheItemType>(CacheKeyType context);
    }
}