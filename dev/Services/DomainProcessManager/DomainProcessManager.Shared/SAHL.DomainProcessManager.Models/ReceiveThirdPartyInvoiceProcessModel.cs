using SAHL.Core.Data;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ReceiveThirdPartyInvoiceProcessModel : IDataModel
    {
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string EmailSubject { get; set; }

        [DataMember]
        public string FileContentAsBase64 { get; set; }

        [DataMember]
        public string FromEmailAddress { get; set; }

        [DataMember]
        public string InvoiceFileExtension { get; set; }

        [DataMember]
        public string InvoiceFileName { get; set; }

        [DataMember]
        public int LoanNumber { get; set; }

        [DataMember]
        public DateTime DateReceived { get; protected set; }

        public ReceiveThirdPartyInvoiceProcessModel(int loanNumber, DateTime dateReceived, string fromEmailAddress, string emailSubject, string invoiceFileName, 
            string invoiceFileExtension, string category, string fileContentAsBase64)
        {
            this.LoanNumber = loanNumber;
            this.FromEmailAddress = fromEmailAddress;
            this.EmailSubject = emailSubject;
            this.InvoiceFileName = invoiceFileName;
            this.InvoiceFileExtension = invoiceFileExtension;
            this.Category = category;
            this.FileContentAsBase64 = fileContentAsBase64;
            this.DateReceived = dateReceived;
        }
    }
}