namespace SAHL.Core.Caching
{
    public class ContextCacheByIDGeneratorFactory : ICacheKeyGeneratorFactory<IKeyedCacheKey>
    {
        public string GetKey<U>(IKeyedCacheKey context)
        {
            return string.Format("{0}_{1}_{2}", context.Context, context.CacheItemType.FullName, context.Key);
        }
    }
}