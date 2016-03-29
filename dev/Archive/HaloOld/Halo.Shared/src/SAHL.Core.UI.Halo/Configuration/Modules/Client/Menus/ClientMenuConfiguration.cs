using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Menus;

namespace SAHL_Config.Website.Halo.Modules.Client.Menus
{
    public class ClientMenuConfiguration : StaticTextMenuItemConfiguration, IMenuItemConfiguration<ClientApplicationModule>
    {
        public ClientMenuConfiguration(ClientApplicationModule applicationModule)
            : base(applicationModule, "ClientModuleAccess", 0, "Client", "Client", "Index")
        {
        }
    }
}