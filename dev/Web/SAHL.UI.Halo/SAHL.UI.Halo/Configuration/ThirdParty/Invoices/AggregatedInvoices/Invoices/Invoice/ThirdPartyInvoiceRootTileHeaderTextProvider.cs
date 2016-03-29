using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice
{
    public class ThirdPartyInvoiceRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoiceRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ThirdPartyInvoiceRootModel;
            if (model == null) { return; }
            this.HeaderText = model.AccountKey.ToString();
        }
    }
}