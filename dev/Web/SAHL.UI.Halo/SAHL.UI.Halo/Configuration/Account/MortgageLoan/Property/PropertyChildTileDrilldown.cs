using SAHL.UI.Halo.Configuration.Account.HOC;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property
{
    public class PropertyChildTileDrilldown : HaloTileActionDrilldownBase<PropertyChildTileConfiguration, PropertyRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<PropertyChildTileConfiguration>
    {
        public PropertyChildTileDrilldown()
            : base("Property")
        {
        }
    }
}