using SAHL.Core.Data;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ReceiveAttorneyInvoiceProcessModel : ReceiveThirdPartyInvoiceProcessModel
    {
        public ReceiveAttorneyInvoiceProcessModel(
              int loanNumber
            , DateTime dateReceived
            , string fromEmailAddress
            , string emailSubject
            , string invoiceFileName
            , string invoiceFileExtension
            , string category
            , string fileContentAsBase64
            )
            : base(loanNumber, dateReceived, fromEmailAddress, emailSubject, invoiceFileName, invoiceFileExtension, category, fileContentAsBase64)
        {
           
        }
    }
}