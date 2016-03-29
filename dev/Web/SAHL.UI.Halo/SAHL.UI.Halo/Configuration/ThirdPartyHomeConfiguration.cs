using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration
{
    public class ThirdPartyHomeConfiguration : HaloModuleConfiguration,
                                            IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>
    {
        public ThirdPartyHomeConfiguration()
            : base("ThirdParty", 1)
        {
        }
    }
}