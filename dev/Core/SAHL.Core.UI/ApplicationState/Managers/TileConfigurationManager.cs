using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.Modules;
using SAHL.Core.UI.Providers.Tiles;
using SAHL.Core.UI.UserState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public class TileConfigurationManager : ITileConfigurationManager
    {
        private IIocContainer iocContainer;
        private ISecurityManager securityManager;

        public TileConfigurationManager(IIocContainer iocContainer, ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
            this.iocContainer = iocContainer;
        }

        public Configuration.IRootTileConfiguration GetRootTileConfigurationForContext(IPrincipal user, string context)
        {
            // get the application module by context
            IApplicationModule applicationModule = this.iocContainer.GetAllInstances<IApplicationModule>().SingleOrDefault(x => x.Name.ToLower() == context);
            if (applicationModule != null)
            {
                // discover the tile configurations for this application module
                Type openGenericRootTileConfigurationType = typeof(IRootTileConfiguration<>);
                Type genericRootTileConfigurationType = openGenericRootTileConfigurationType.MakeGenericType(applicationModule.GetType());
                IRootTileConfiguration majorTileConfig = this.iocContainer.GetInstance(genericRootTileConfigurationType) as IRootTileConfiguration;

                return majorTileConfig;
            }

            return null;
        }

        public IEnumerable<IChildTileConfiguration> GetChildConfigurationsForMajorTile(IMajorTileConfiguration majorTileConfiguration)
        {
            if (majorTileConfiguration == null)
            {
                return new IChildTileConfiguration[] { };
            }

            Type openGenericParentedTileConfigurationType = typeof(IParentedTileConfiguration<>);
            Type genericParentedTileConfigurationType = openGenericParentedTileConfigurationType.MakeGenericType(majorTileConfiguration.GetType());
            var childTileConfigs = this.iocContainer.GetAllInstances(genericParentedTileConfigurationType).Cast<IChildTileConfiguration>();
            return childTileConfigs ?? new IChildTileConfiguration[] { };
        }

        public IEnumerable<IActionTileConfiguration> GetActionTileConfigurationsForMajorTile(IMajorTileConfiguration majorTileConfiguration)
        {
            if (majorTileConfiguration == null)
            {
                return new IActionTileConfiguration[] { };
            }

            Type openGenericParentedTileConfigurationType = typeof(IParentedActionTileConfiguration<>);
            Type genericParentedTileConfigurationType = openGenericParentedTileConfigurationType.MakeGenericType(majorTileConfiguration.GetType());
            var actionTileConfigs = this.iocContainer.GetAllInstances(genericParentedTileConfigurationType).Cast<IActionTileConfiguration>();
            return actionTileConfigs ?? new IActionTileConfiguration[] { };
        }

        public ITileDataProvider GetTileDataProviderForTileModel(Type tileModelType)
        {
            // get the data provider
            Type openGenericTileDataProvider = typeof(ITileDataProvider<>);
            Type genericTileDataProvider = openGenericTileDataProvider.MakeGenericType(tileModelType);
            var tileDataProvider = this.iocContainer.GetInstance(genericTileDataProvider) as ITileDataProvider;

            return tileDataProvider;
        }

        public ITileContentProvider GetTileContentProviderForTileModel(Type tileModelType)
        {
            // get the content provider
            Type openGenericTileContentProvider = typeof(ITileContentProvider<>);
            Type genericTileContentProvider = openGenericTileContentProvider.MakeGenericType(tileModelType);
            var tileContentProvider = this.iocContainer.GetInstance(genericTileContentProvider) as ITileContentProvider;

            return tileContentProvider;
        }

        public IMajorTileConfiguration GetDrillDownConfigurationForClickedTile(Type clickedTileConfigurationType)
        {
            // get the major tile configuration for the drill down operation
            Type openGenericTileDataProvider = typeof(IDrillDownTileConfiguration<>);
            Type genericTileDataProvider = openGenericTileDataProvider.MakeGenericType(clickedTileConfigurationType);
            var majorTileConfiguration = this.iocContainer.GetInstance(genericTileDataProvider) as IMajorTileConfiguration;

            return majorTileConfiguration;
        }

        public IMajorTileConfiguration GetTileConfigurationFromType(Type contextModelType)
        {
            Type openGenericMajorTileConfig = typeof(IMajorTileConfiguration<>);
            Type genericMajorTileConfig = openGenericMajorTileConfig.MakeGenericType(contextModelType);
            var majorTileConfiguration = this.iocContainer.GetInstance(genericMajorTileConfig) as IMajorTileConfiguration;

            return majorTileConfiguration;
        }

        public Type GetAlternateTileModelIfConfiguredForUser(Type defaultTileModel, IUserPrincipal userPrincipal)
        {
            Type openAlternateTileModelGeneric = typeof(IAlternateTileModel<>);
            Type alternateTileModelGeneric = openAlternateTileModelGeneric.MakeGenericType(defaultTileModel);
            var alternateTileModel = this.iocContainer.GetInstance(alternateTileModelGeneric, userPrincipal.ActiveRole.OrganisationArea + "." + userPrincipal.ActiveRole.RoleName);
            return alternateTileModel != null ? alternateTileModel.GetType() : null;
        }
    }
}