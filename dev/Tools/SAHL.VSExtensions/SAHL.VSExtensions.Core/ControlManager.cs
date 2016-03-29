using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using StructureMap;
using System;

namespace SAHL.VSExtensions.Core
{
    public class ControlManager : IControlManager
    {
        private IContainer iocContainer;

        public ControlManager(IContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public object GetUserControlForMenuItem(IMenuItem menuItem)
        {
            Type menuItemType = menuItem.GetType();
            Type controlConfig = typeof(ISAHLControlForConfiguration<>);
            controlConfig = controlConfig.MakeGenericType(menuItemType);
            return iocContainer.GetInstance(controlConfig);
        }
    }
}