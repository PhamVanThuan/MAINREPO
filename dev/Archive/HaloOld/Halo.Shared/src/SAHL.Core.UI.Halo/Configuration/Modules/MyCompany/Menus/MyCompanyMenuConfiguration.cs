using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Menus;

namespace SAHL_Config.Website.Halo.MyCompany.Modules.MyCompany.Menus
{
    public class MyCompanyMenuConfiguration : StaticTextMenuItemConfiguration, IMenuItemConfiguration<MyCompanyApplicationModule>
    {
        public MyCompanyMenuConfiguration(MyCompanyApplicationModule applicationModule)
            : base(applicationModule, "MyCompanyModuleAccess", 40, "My Company", "MyCompany", "Index")
        {
        }
    }
}