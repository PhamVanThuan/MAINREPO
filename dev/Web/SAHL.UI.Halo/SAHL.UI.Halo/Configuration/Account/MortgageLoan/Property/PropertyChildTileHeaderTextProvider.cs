using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property
{
    public class PropertyChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<PropertyChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Property";
        }
    }
}