using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoiceDetailRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ThirdPartyInvoiceDetailRootModel;
            if (model == null)
            { return; }
            this.HeaderText = model.LegalName;
        }
    }
}