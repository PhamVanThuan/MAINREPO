using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoiceDetailChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ThirdPartyInvoiceDetailModel;
            if (model == null)
            { return; }
            this.HeaderText = model.AccountKey.ToString();
        }
    }
}