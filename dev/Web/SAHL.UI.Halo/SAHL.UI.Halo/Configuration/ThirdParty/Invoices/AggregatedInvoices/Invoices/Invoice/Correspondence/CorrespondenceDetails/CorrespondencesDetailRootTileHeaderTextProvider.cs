using SAHL.UI.Halo.Shared.Configuration.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence.CorrespondenceDetails
{
    public class CorrespondencesDetailRootTileHeaderTextProvider : HaloTileHeaderTextProviderBase<CorrespondencesDetailRootTileHeaderConfiguration>
    {
        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            this.HeaderText = "Correspondence";
        }
    }
}
