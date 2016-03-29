using SAHL.Core.Data;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;

namespace SAHL.Services.DocumentManager.Managers.Document.Statements
{
    public class InsertClientFileDocumentDataStatement : ISqlStatement<ClientFileDocumentModel>
    {
        public int Store { get; protected set; }

        public string DataStoreGuid { get; protected set; }

        public string Extension { get; protected set; }

        public string IdentityNumber { get; protected set; }

        public string Firstname { get; protected set; }

        public string Surname { get; protected set; }

        public string Category { get; protected set; }

        public string OriginalFileName { get;protected set; }

        public string Username { get; protected set; }

        public string CreatedDate { get; protected set; }
        
        public string ArchiveDate { get; protected set; }

        public DateTime MsgSentDate { get; protected set; }

        public DateTime MsgReceivedDate { get; protected set; }

        public string EmptyKey { get; protected set; }

        public InsertClientFileDocumentDataStatement(ClientFileDocumentModel document, FileExtension extension, string dataStoreGuid, string originalFileName, string username,
            int storeId, string documentInsertDate, string archiveDate, DateTime msgDate)
        {
            this.Store = storeId;
            this.DataStoreGuid = dataStoreGuid;
            this.Extension = extension.ToString();
            this.IdentityNumber = document.IdentityNumber;
            this.Firstname = document.FirstName;
            this.Surname = document.Surname;
            this.OriginalFileName = originalFileName;
            this.Category = document.Category;
            this.CreatedDate = documentInsertDate;
            this.Username = username;
            this.ArchiveDate = archiveDate;
            this.MsgReceivedDate = msgDate;
            this.MsgSentDate = msgDate;
            this.EmptyKey = "";
        }

        // Key1 = Identity number
        // Key4 = Document type
        // Key5 = Firstname
        // Key6 = Surname
        // Key7 = Date created
        // Key8 = Username
        public string GetStatement()
        {
            return @"insert into [ImageIndex].[dbo].[Data]
                       ([archiveDate],[dataContainer],[STOR],[GUID],[Extension]
                       ,[Key1],[Key2],[Key3],[Key4],[Key5]
                       ,[Key6],[Key7],[Key8],[Key9],[Key10]
                       ,[Key11],[OriginalFilename],[MsgReceived],[MsgSent])
                 Values
                       (@ArchiveDate, 0, @Store, @DataStoreGuid, @Extension,
                        @IdentityNumber, @EmptyKey, @EmptyKey, @Category, @Firstname,
                        @Surname, @CreatedDate, @Username, @EmptyKey, @EmptyKey,
                        @EmptyKey, @OriginalFileName, @MsgReceivedDate, @MsgSentDate)";
        }
    }
}