using SAHL.UI.Halo.Models.Common.Correspondence;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence
{
    public class CorrespondenceChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyInvoiceRootTileConfiguration>,
                                                            IHaloTileModel<CorrespondenceChildModel>
    {
        public CorrespondenceChildTileConfiguration()
            : base("Correspondence", "Correspondence", 7, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}