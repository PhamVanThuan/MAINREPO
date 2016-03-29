using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations
{


    public class PreviousValuationsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<PropertyRootTileConfiguration>,
                                                    IHaloTileModel<PreviousValuationsModel>
    {

        public PreviousValuationsChildTileConfiguration()
            : base("PreviousValuationsRoot", "PreviousValuationsRoot", 4, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}