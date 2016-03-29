using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetLossControlAttorneyInvoiceStorDataQueryResult
    {
        public GetLossControlAttorneyInvoiceStorDataQueryResult(decimal id,
            string archivedate,
            decimal stor,
            string guid,
            string extension,
            string accountKey,
            string thirdPartyInvoiceKey,
            string emailSubject,
            string fromEmailAddress,
            string invoiceFileName,
            string category,
            string datereceived,
            string dateProcessed,
            string filePath)
        {
            this.Id = Convert.ToInt32(id);
            this.ArchiveDate = archivedate;
            this.Stor = Convert.ToInt32(stor);
            this.Guid = guid;
            this.Extension = extension;
            this.AccountKey = Convert.ToInt32(accountKey);
            this.ThirdPartyInvoiceKey = Convert.ToInt32(thirdPartyInvoiceKey);
            this.EmailSubject = emailSubject;
            this.FromEmailAddress = fromEmailAddress;
            this.InvoiceFileName = invoiceFileName;
            this.Category = category;
            this.DateReceived = Convert.ToDateTime(datereceived);
            this.DateProcessed = Convert.ToDateTime(dateProcessed);
            this.FilePath = filePath;
        }

        public int Id { get; set; }

        public string ArchiveDate { get; set; }

        public int Stor { get; set; }

        public string Guid { get; set; }

        public string Extension { get; set; }

        public int AccountKey { get; set; }

        public int ThirdPartyInvoiceKey { get; set; }

        public string EmailSubject { get; set; }

        public string FromEmailAddress { get; set; }

        public string InvoiceFileName { get; set; }

        public string Category { get; set; }

        public DateTime DateReceived { get; set; }

        public DateTime DateProcessed { get; set; }

        public string FilePath { get; set; }
    }
}