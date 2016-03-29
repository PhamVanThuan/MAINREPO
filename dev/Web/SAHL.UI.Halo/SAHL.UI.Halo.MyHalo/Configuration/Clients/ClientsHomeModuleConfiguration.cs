using SAHL.UI.Halo.MyHalo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Home.Configuration.Clients
{
    public class ClientsHomeModuleConfiguration : HaloModuleConfiguration,
                                                  IHaloModuleApplicationConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public ClientsHomeModuleConfiguration() : base("Clients", 1)
        {
        }
    }
}
