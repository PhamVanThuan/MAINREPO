using SAHL.UI.Halo.Shared.Configuration.Header;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence
{
    public class ThirdPartyInvoiceCorrespondencesChildTileHeaderTextProvider : HaloTileHeaderTextProviderBase<ThirdPartyInvoiceCorrespondencesChildTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Correspondence";
        }
    }
}