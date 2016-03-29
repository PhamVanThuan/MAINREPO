using SAHL.Core.Caching;
using SAHL.Core.UI.ApplicationState.Managers;

namespace SAHL.Core.UI.Caching
{
    public class ApplicationStateCacheKeyConfigurationFactory : AbstractCacheKeyGeneratorFactory<IApplicationStateManager>
    {
        public ApplicationStateCacheKeyConfigurationFactory(IHashGenerator hashGenerator)
            : base(hashGenerator)
        {
        }

        protected override string GetContextString(IApplicationStateManager context)
        {
            return context.ApplicationName;
        }
    }
}