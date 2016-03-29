using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.UI.ApplicationState.Models;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Elements.Areas;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SAHL.Core.UI.UserState.Managers
{
    public class MenuManager : IMenuManager
    {
        private IApplicationStateManager applicationStateManager;

        public MenuManager(IApplicationStateManager applicationStateManager)
        {
            this.applicationStateManager = applicationStateManager;
        }

        public MenuElementArea CreateMenuForUser(IPrincipal user)
        {
            // get the list of menuitems from the application state
            Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>> configuredMenuItems = new Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>>();

            ApplicationConfiguration configuration = this.applicationStateManager.Configuration;

            foreach (var kvp in configuration.AvailableMenus.OrderBy(x => x.Key.Sequence))
            {
                var menuItemConfig = kvp.Key;
                var ribbonItemConfigs = kvp.Value.OrderBy(x => x.Sequence);

                configuredMenuItems.Add(menuItemConfig, ribbonItemConfigs);
            }

            MenuElementArea menuElementArea = new MenuElementArea(configuredMenuItems);
            return menuElementArea;
        }
    }
}