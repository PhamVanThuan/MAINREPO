using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Modules;
using System.Collections.Generic;

namespace SAHL.Core.UI.ApplicationState.Models
{
    public class ApplicationConfiguration
    {
        private Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>> availableMenuItems;
        private List<IApplicationModule> availableModules;

        public ApplicationConfiguration()
        {
            this.availableModules = new List<IApplicationModule>();
            this.availableMenuItems = new Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>>();
        }

        public Dictionary<IMenuItemConfiguration, IEnumerable<IRibbonItemConfiguration>> AvailableMenus { get { return this.availableMenuItems; } }

        public IList<IApplicationModule> AvailableApplicationModules { get { return this.availableModules; } }
    }
}