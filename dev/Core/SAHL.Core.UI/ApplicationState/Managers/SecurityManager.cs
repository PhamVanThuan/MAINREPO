using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.UserState.Managers;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public class SecurityManager : ISecurityManager
    {
        private IIocContainer iocContainer;

        public SecurityManager(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public T CanAccess<T>(IPrincipal user, IRequiredFeatureAccess majorTileConfig) where T : class, IRequiredFeatureAccess
        {
            try
            {
                IUserStateManager userStatemanger = this.iocContainer.GetInstance(typeof(IUserStateManager)) as IUserStateManager;
                var features = userStatemanger.GetFeatureAccessForUser(user);
                if (features.Any(x => x.FeatureName == (majorTileConfig as IRequiredFeatureAccess).RequiredFeatureAccess))
                {
                    return majorTileConfig as T;
                }
            }
            catch
            {
                throw;
            }

            return new NoAccessConfiguration() as T;
        }
    }

    public interface ISecurityManager
    {
        T CanAccess<T>(IPrincipal user, IRequiredFeatureAccess majorTileConfig) where T : class, IRequiredFeatureAccess;
    }
}