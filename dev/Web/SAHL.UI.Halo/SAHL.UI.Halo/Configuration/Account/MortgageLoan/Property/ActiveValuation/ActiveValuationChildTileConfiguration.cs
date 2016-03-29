using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.ActiveValuation
{


    public class ActiveValuationChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<PropertyRootTileConfiguration>,
                                                    IHaloTileModel<ActiveValuationModel>
    {

        public ActiveValuationChildTileConfiguration()
            : base("ActiveValuation", "Active Valuation",2, noOfRows:2, noOfColumns:3)
        {
        }
    }
}