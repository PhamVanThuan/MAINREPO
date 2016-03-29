using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PayThirdPartyInvoiceProcessModel : IDataModel
    {
        [DataMember]
        public List<PayThirdPartyInvoiceModel> InvoiceCollection { get; set; }

        [DataMember]
        public string UserName { get; protected set; }

        public PayThirdPartyInvoiceProcessModel(List<PayThirdPartyInvoiceModel> invoiceCollection, string username)
        {
            this.InvoiceCollection = invoiceCollection;
            this.UserName = username;
        }
    }
}