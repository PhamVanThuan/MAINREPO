using SAHL.UI.Halo.Models.Common.Correspondence;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence
{
    public class ThirdPartyInvoiceCorrespondencesChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                            IHaloTileModel<CorrespondenceChildModel>
    {
        public ThirdPartyInvoiceCorrespondencesChildTileConfiguration()
            : base("Correspondence", "Correspondence", 3, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}