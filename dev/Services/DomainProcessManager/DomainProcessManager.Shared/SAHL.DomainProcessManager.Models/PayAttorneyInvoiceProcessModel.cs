using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PayAttorneyInvoiceProcessModel : PayThirdPartyInvoiceProcessModel
    {
        public PayAttorneyInvoiceProcessModel(List<PayThirdPartyInvoiceModel> invoiceCollection, string username)
            : base(invoiceCollection, username)
        {

        }
    }
}
