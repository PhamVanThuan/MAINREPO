using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration
{
    public class ClientHomeConfiguration : HaloModuleConfiguration,
                                            IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>
    {
        public ClientHomeConfiguration()
            : base("Clients", 1)
        {
        }
    }
}