using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class GetThirdPartyInvoiceCorrespondenceQueryResult
    {
        public int Id { get; set; }
        public string CorrespondenceType { get; set; }
        public string CorrespondenceReason { get; set; }
        public string CorrespondenceMedium { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string QueryText { get; set; }
        public int ThirdPartyInvoiceKey { get; set; }
        public int GenericKeyTypeKey { get; set; }
    }
}
