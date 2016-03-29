using SAHL.UI.Halo.Models.Common.Correspondence;
using SAHL.UI.Halo.Pages.Common.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence.CorrespondenceDetails
{
    public class CorrespondencesDetailRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ThirdPartyHomeConfiguration>,
                                                     IHaloModuleTileConfiguration<TaskHomeConfiguration>,
                                                     IHaloTileModel<CorrespondenceChildModel>,
                                                     IHaloTilePageState<ThirdPartyCorrespondencePageState>
    {
        public CorrespondencesDetailRootTileConfiguration()
            : base("Invoice Correspondence", "CorrespondencesDetail", 5, false)
        {
        }
    }
}
