using SAHL.Core.Caching;
using SAHL.Core.UI.ApplicationState.Models;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Modules;
using System;
using System.Linq;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public class ApplicationStateManager : IApplicationStateManager
    {
        private ICache cache;
        private ICacheKeyGenerator cacheKeyGenerator;
        private IIocContainer iocContainer;

        public ApplicationStateManager(ICache cache, ICacheKeyGenerator cacheKeyGenerator, IIocContainer iocContainer)
        {
            this.cache = cache;
            this.cacheKeyGenerator = cacheKeyGenerator;
            this.iocContainer = iocContainer;
        }

        public string ApplicationName
        {
            get
            {
                return "Halo";
            }
        }

        public ApplicationConfiguration Configuration
        {
            get
            {
                string cacheKey = cacheKeyGenerator.GetKey<IApplicationStateManager, ApplicationConfiguration>(this);
                return this.cache.GetItem<ApplicationConfiguration>(cacheKey);
            }
        }

        public void Start()
        {
            // initialise the configuration
            var configuration = new ApplicationConfiguration();
            var applicationModules = iocContainer.GetAllInstances<IApplicationModule>();

            foreach (var applicationModule in applicationModules)
            {
                configuration.AvailableApplicationModules.Add(applicationModule);
            }

            string cacheKey = cacheKeyGenerator.GetKey<IApplicationStateManager, ApplicationConfiguration>(this);
            this.cache.AddItem<ApplicationConfiguration>(cacheKey, configuration);

            foreach (var applicationModule in applicationModules)
            {
                // discover the menus configured for this appliction module
                Type moduleType = applicationModule.GetType();
                Type openGenericMenuItemConfig = typeof(IMenuItemConfiguration<>);
                Type genericMenuItemConfig = openGenericMenuItemConfig.MakeGenericType(moduleType);
                var menuItemConfigs = iocContainer.GetAllInstances(genericMenuItemConfig).Cast<IMenuItemConfiguration>().OrderBy(x => x.Sequence);

                foreach (var menuItemConfig in menuItemConfigs)
                {
                    Type menuItemConfigType = menuItemConfig.GetType();
                    Type openGenericRibbonItemConfig = typeof(IRibbonItemConfiguration<>);
                    Type genericRibbonItemConfig = openGenericRibbonItemConfig.MakeGenericType(menuItemConfigType);
                    var ribbonItemConfigs = iocContainer.GetAllInstances(genericRibbonItemConfig).Cast<IRibbonItemConfiguration>().OrderBy(x => x.Sequence);

                    this.Configuration.AvailableMenus.Add(menuItemConfig, ribbonItemConfigs);
                }
            }
        }
    }
}