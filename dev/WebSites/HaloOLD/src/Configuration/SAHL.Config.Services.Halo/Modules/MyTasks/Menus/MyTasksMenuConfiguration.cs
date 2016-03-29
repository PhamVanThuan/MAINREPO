using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Menus;

namespace SAHL_Config.Website.Halo.MyTasks.Modules.MyTasks.Menus
{
    public class MyTasksMenuConfiguration : StaticTextMenuItemConfiguration, IMenuItemConfiguration<MyTasksApplicationModule>
    {
        public MyTasksMenuConfiguration(MyTasksApplicationModule applicationModule)
            : base(applicationModule, "MyTasksModuleAccess", 10, "My Tasks", "MyTasks", "Index")
        {
        }
    }
}