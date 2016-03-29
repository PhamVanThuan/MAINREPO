using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Models.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.DeedsOfficeDetails
{


    public class DeedsOfficeDetailsChildTileConfiguration : HaloSubTileConfiguration,
                                                    IHaloChildTileConfiguration<PropertyRootTileConfiguration>,
                                                    IHaloTileModel<DeedsOfficeDetailsModel>
    {

        public DeedsOfficeDetailsChildTileConfiguration()
        : base("DeedsOfficeDetails", "Deeds Office Details", 3, noOfRows: 2, noOfColumns: 3)
        {
        }
    }
}