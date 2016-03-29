using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class ThirdPartyInvoiceQueryRootModel : IHaloTileModel
    {
        public string CorrespondenceMedium { get; set; }
        public string CorrespondenceReason { get; set; }
        public string CorrespondenceType { get; set; }
        public DateTime Date { get; set; }
        public int GenericKeyTypeKey { get; set; }
        public int Id { get; set; }
        public string QueryText { get; set; }
        public int ThirdPartyInvoiceKey { get; set; }
        public string UserName { get; set; }
    }
}
