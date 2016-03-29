using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence.CorrespondenceDetails;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries.ThirdPartyInvoiceQueriesDetails;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence
{
    public class ThirdPartyInvoiceCorrespondencesChildPageDrilldown : HaloTileActionDrilldownBase<ThirdPartyInvoiceCorrespondencesChildTileConfiguration, CorrespondencesDetailRootTileConfiguration>,
                                                        IHaloTileActionDrilldown<ThirdPartyInvoiceCorrespondencesChildTileConfiguration>
    {
        public ThirdPartyInvoiceCorrespondencesChildPageDrilldown()
            : base("Correspondence")
        {
        }
    }
}
