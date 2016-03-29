using SAHL.Core.Caching;
using SAHL.Core.UI.Context;

namespace SAHL.Core.UI.Caching
{
    public class PrincipalTileBusinessContextCacheGeneratorFactory : ICacheKeyGeneratorFactory<PrincipalTileBusinessContext>
    {
        public string GetKey<U>(PrincipalTileBusinessContext context)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", context.User.Identity.Name.ToLower(), context.BusinessContext.Context.ToLower(),
                context.BusinessContext.BusinessKey.KeyType, context.BusinessContext.BusinessKey.Key,
                context.BusinessContext.TileModelType != null ? context.BusinessContext.TileModelType.ToString() : "none",
                context.BusinessContext.TileConfigurationType != null ? context.BusinessContext.TileConfigurationType.ToString() : "none");
        }
    }
}