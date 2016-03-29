using SAHL.UI.Halo.Models.ThirdParty;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty
{
    public class ThirdPartyRootTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloRootTileConfiguration,
                                                    IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                    IHaloTileModel<ThirdPartyRootModel>
    {
        public ThirdPartyRootTileConfiguration()
            : base("Third Party", "ThirdParty", 1)
        {
        }
    }
}