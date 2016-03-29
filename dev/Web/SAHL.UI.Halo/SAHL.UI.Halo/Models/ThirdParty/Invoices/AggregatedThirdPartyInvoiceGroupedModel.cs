using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class AggregatedThirdPartyInvoiceGroupedModel : IHaloTileModel
    {
        public int InvoiceStatusKey { get; set; }

        public string InvoiceStatusDescription { get; set; }

        public int NumberOfInvoices { get; set; }
    }
}