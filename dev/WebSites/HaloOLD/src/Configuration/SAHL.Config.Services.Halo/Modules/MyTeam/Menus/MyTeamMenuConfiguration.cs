using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Menus;

namespace SAHL_Config.Website.Halo.MyTeam.Modules.MyTeam.Menus
{
    public class MyTeamMenuConfiguration : StaticTextMenuItemConfiguration, IMenuItemConfiguration<MyTeamApplicationModule>
    {
        public MyTeamMenuConfiguration(MyTeamApplicationModule applicationModule)
            : base(applicationModule, "MyTeamModuleAccess", 30, "My Team", "MyTeam", "Index")
        {
        }
    }
}