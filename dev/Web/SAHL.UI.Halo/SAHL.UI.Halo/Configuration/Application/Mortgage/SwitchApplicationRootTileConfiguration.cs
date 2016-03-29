using SAHL.UI.Halo.Models.Application;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Application.Mortgage
{
    public class SwitchApplicationRootTileConfiguration : HaloSubTileConfiguration,
                                                          IHaloAlternativeRootTileConfiguration<ApplicationRootTileConfiguration>,
                                                          IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                          IHaloTileModel<SwitchApplicationRootModel>
    {
        public SwitchApplicationRootTileConfiguration()
            : base("Switch Application", "SwitchApplication", 2)
        {
        }
    }
}
