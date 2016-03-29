using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration.Header;
using System;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails
{
    public class PropertyRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PropertyRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Property Details";
        }
    }
}