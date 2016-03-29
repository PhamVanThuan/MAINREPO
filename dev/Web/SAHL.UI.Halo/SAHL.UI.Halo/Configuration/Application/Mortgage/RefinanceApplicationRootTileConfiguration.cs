using SAHL.UI.Halo.Models.Application;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Application.Mortgage
{
    public class RefinanceApplicationRootTileConfiguration : HaloSubTileConfiguration,
                                                             IHaloAlternativeRootTileConfiguration<ApplicationRootTileConfiguration>,
                                                             IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                             IHaloTileModel<RefinanceApplicationRootModel>
    {
        public RefinanceApplicationRootTileConfiguration()
            : base("Refinance Application", "RefinanceApplication", 2)
        {
        }
    }
}
