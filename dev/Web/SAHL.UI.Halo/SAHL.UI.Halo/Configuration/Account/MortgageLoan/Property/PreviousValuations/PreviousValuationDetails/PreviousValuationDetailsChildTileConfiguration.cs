using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PropertyDetails;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PreviousValuations.PreviousValuationDetails
{
    public class PreviousValuationDetailsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<PreviousValuationsRootTileConfiguration>,
                                                    IHaloTileModel<PreviousValuationModel>
    {
        public PreviousValuationDetailsChildTileConfiguration()
            : base("Previous Valuation", "PreviousValuation", 2, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}