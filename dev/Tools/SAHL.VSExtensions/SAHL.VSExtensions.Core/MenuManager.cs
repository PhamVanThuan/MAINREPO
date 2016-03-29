using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.VSExtensions.Core
{
    public class MenuManager : IMenuManager
    {
        private IEnumerable<ISAHLConfiguration> MenuItems { get; set; }

        public MenuManager(ISAHLConfiguration[] menuItems)
        {
            this.MenuItems = menuItems;
        }

        public IEnumerable<string> GetMenuGroups()
        {
            return MenuItems.Select(x => x.GroupName).Distinct();
        }

        public IEnumerable<IMenuItem> GetMenuItems(string groupName)
        {
            return MenuItems.Where(x => x.GroupName == groupName);
        }
    }
}