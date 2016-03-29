using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.DeedsOfficeDetails
{
    public class DeedsOfficeDetailsChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<DeedsOfficeDetailsChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = " Deeds Office Details";
        }
    }
}