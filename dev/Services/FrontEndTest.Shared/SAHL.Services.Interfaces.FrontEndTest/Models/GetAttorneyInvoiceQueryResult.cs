using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetAttorneyInvoiceQueryResult
    {
        public GetAttorneyInvoiceQueryResult(decimal id, decimal stor, string guid, string extension, string loanNumber, string thirdPartyInvoiceKey, string emailSubject, string fromEmailAddress,
            string invoiceFileName, string category, string dateRecieved, string dateProcess)
        {
            this.ID = Convert.ToInt32(id);
            this.STOR = Convert.ToInt32(stor);
            this.GUID = guid;
            this.Extension = extension;
            this.LoanNumber = loanNumber;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.EmailSubject = emailSubject;
            this.FromEmailAddress = fromEmailAddress;
            this.InvoiceFileName = invoiceFileName;
            this.Category = category;
            this.DateRecieved = dateRecieved;
            this.DateProcess = dateProcess;
        }

        public int ID { get; set; }

        public int STOR { get; set; }

        public string GUID { get; set; }

        public string Extension { get; set; }

        public string LoanNumber { get; set; }

        public string ThirdPartyInvoiceKey { get; set; }

        public string EmailSubject { get; set; }

        public string FromEmailAddress { get; set; }

        public string InvoiceFileName { get; set; }

        public string Category { get; set; }

        public string DateRecieved { get; set; }

        public string DateProcess { get; set; }
    }
}