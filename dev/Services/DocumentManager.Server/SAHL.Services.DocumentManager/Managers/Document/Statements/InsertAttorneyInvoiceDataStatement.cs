using System;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.DocumentManager.Models;

namespace SAHL.Services.DocumentManager.Managers.Document.Statements
{
    public class InsertAttorneyInvoiceDataStatement : ISqlStatement<ThirdPartyInvoiceDocumentModel>
    {

        public int Store { get; protected set; }

        public string DataStoreGuid { get; protected set; }

        public string Extension { get; protected set; }

        public string LoanNumber { get; protected set; }

        public string ThirdPartyInvoiceKey { get; protected set; }

        public DateTime DateReceived { get; protected set; }

        public DateTime DateProcessed { get; protected set; }

        public string FromEmailAddress { get; protected set; }

        public string EmailSubject { get; protected set; }

        public string InvoiceFileName { get; protected set; }

        public string Category { get; protected set; }

        public string OriginalFileName { get; protected set; }

        public string DateArchived { get; protected set; }

        public InsertAttorneyInvoiceDataStatement(ThirdPartyInvoiceDocumentModel invoice, string thirdPartyInvoiceKey, string dataStoreGuid, string originalFileName, 
            int storeId, string dateArchived)
        {
            this.Store = storeId;
            this.DataStoreGuid = dataStoreGuid;
            this.Extension = invoice.InvoiceFileExtension;
            this.LoanNumber = invoice.LoanNumber;
            this.FromEmailAddress = invoice.FromEmailAddress;
            this.EmailSubject = invoice.EmailSubject;
            this.DateReceived = invoice.DateReceived;
            this.DateProcessed = invoice.DateProcessed;
            this.InvoiceFileName = invoice.InvoiceFileName;
            this.Extension = invoice.InvoiceFileExtension;
            this.OriginalFileName = originalFileName;
            this.Category = invoice.Category;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.DateArchived = dateArchived;
        }
        
        public string GetStatement()
        {
            return @"insert into [ImageIndex].[dbo].[Data]
                       ([dataContainer]
                       ,[archiveDate]
                       ,[STOR]
                       ,[GUID]
                       ,[Extension]
                       ,[OriginalFilename]                       
                       ,[Key1]
                       ,[Key2]
                       ,[Key3]
                       ,[Key4]
                       ,[Key5]
                       ,[Key6]
                       ,[Key7]
                       ,[Key8]
                       )
                 Values
                       (0,@DateArchived,@Store,@DataStoreGuid,@Extension,@OriginalFileName,@LoanNumber,@ThirdPartyInvoiceKey,@EmailSubject,@FromEmailAddress,
                        @InvoiceFileName,@Category,@DateReceived,@DateProcessed)";
        }
    }
}