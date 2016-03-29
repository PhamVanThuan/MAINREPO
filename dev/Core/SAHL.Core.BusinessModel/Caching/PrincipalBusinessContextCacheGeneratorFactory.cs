using SAHL.Core.Caching;

namespace SAHL.Core.BusinessModel.Caching
{
    public class PrincipalBusinessContextCacheGeneratorFactory : ICacheKeyGeneratorFactory<PrincipalBusinessContext>
    {
        public string GetKey<U>(PrincipalBusinessContext context)
        {
            return string.Format("{0}_{1}_{2}_{3}", context.User.Identity.Name.ToLower(), context.BusinessContext.Context.ToLower(),
                context.BusinessContext.BusinessKey.KeyType, context.BusinessContext.BusinessKey.Key);
        }
    }
}