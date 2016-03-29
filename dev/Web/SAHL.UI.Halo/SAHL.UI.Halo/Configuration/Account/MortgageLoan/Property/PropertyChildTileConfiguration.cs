using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property
{
    public class PropertyChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<MortgageLoanRootTileConfiguration>,
                                                      IHaloTileModel<PropertyChildModel>
    {
        public PropertyChildTileConfiguration()
        :base("Property", "Property", 2, noOfRows: 2, noOfColumns: 3)
        
            
        {
        }
    }
}