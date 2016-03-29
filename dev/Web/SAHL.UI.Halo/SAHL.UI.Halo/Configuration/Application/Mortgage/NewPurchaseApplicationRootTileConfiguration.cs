using SAHL.UI.Halo.Models.Application;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Application.Mortgage
{
    public class NewPurchaseApplicationRootTileConfiguration : HaloSubTileConfiguration,
                                                               IHaloAlternativeRootTileConfiguration<ApplicationRootTileConfiguration>,
                                                               IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                               IHaloTileModel<NewPurchaseApplicationRootModel>
    {
        public NewPurchaseApplicationRootTileConfiguration()
            : base("New Purchase Application", "NewPurchaseApplication")
        {
        }
    }
}
