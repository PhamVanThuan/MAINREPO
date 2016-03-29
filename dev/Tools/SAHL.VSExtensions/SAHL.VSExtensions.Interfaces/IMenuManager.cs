using SAHL.VSExtensions.Interfaces.Configuration;
using System.Collections.Generic;

namespace SAHL.VSExtensions.Interfaces
{
    public interface IMenuManager
    {
        IEnumerable<string> GetMenuGroups();

        IEnumerable<IMenuItem> GetMenuItems(string groupName);
    }
}