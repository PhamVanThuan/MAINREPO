using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices
{
    public class ThirdPartyInvoicesChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ThirdPartyRootTileConfiguration>,
                                                            IHaloTileModel<ThirdPartyInvoiceChildModel>
    {
        public ThirdPartyInvoicesChildTileConfiguration()
            : base("Invoices", "ThirdPartyInvoices", sequence: 3, startRow: 1, noOfRows: 1, noOfColumns: 1)
        {
        }
    }
}