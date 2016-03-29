namespace SAHL.Core.Caching
{
    public class ContextCacheByNameGeneratorFactory : ICacheKeyGeneratorFactory<INamedCacheKey>
    {
        public string GetKey<U>(INamedCacheKey context)
        {
            return string.Format("{0}_{1}_{2}", context.Context, context.CacheItemType.FullName, context.Name);
        }
    }
}