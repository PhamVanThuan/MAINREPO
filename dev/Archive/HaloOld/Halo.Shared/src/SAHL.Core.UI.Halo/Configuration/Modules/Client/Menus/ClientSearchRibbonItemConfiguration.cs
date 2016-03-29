using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Menus;
using SAHL_Config.Website.Halo.Modules.Client.Menus;

namespace SAHL_Config.Website.Halo.Modules.Client
{
    public class ClientSearchRibbonItemConfiguration : StaticSearchRibbonMenuItemConfiguration, IRibbonItemConfiguration<ClientMenuConfiguration>
    {
        public ClientSearchRibbonItemConfiguration(ClientApplicationModule applicationModule)
            : base(applicationModule, 0, "Client", "Search","ClientSearchRibbonAccess")
        {
        }
    }
}