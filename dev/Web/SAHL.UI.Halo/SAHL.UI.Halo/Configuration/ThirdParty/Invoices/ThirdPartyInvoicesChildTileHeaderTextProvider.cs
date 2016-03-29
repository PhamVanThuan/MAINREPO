using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices
{
    public class ThirdPartyInvoicesChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoicesChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Invoices";
        }
    }
}