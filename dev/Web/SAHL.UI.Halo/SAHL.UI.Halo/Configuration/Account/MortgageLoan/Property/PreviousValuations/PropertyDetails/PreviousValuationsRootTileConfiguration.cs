using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PropertyDetails
{
    public class PreviousValuationsRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<PropertyRootModel>
    {
        public PreviousValuationsRootTileConfiguration()
            : base("Previous Valuations", "PreviousValuations", 4, noOfRows: 2)
        {
        }
    }
}