using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.Document.Statements;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;

namespace SAHL.Services.DocumentManager.Specs.Managers.Document
{
    public class when_saving_a_client_file_document : WithCoreFakes
    {
        private static DocumentDataManager dataManager;
        private static ClientFileDocumentModel document;
        private static string originalFileName;
        private static string dataStoreGuid;
        private static int storeID;
        private static string username;
        private static FakeDbFactory dbFactory;
        private static DateTime insertDate;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new DocumentDataManager(dbFactory);
            storeID = 17;
            username = "SAHL\\Zorro";
            insertDate = new DateTime(2014, 12, 15, 8, 9, 10);
            originalFileName = "1234567890123 - Identity documents - 20140812 102536.pdf";
            dataStoreGuid = "{9a885e00-c4e9-4695-9903-73c574df3adb}";
            document = new ClientFileDocumentModel("123213048587432oeukoNTHEou", "Identity documents", "1234567890123", "Bob", "Builder", username, DateTime.Now, FileExtension.Pdf);
        };

        private Because of = () =>
        {
            dataManager.SaveClientFile(document, dataStoreGuid, originalFileName, insertDate);
        };

        private It should_insert_a_client_file_document_with_the_documents_details = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ISqlStatement<ClientFileDocumentModel>>.Matches(y =>
                ((InsertClientFileDocumentDataStatement)y).Firstname == document.FirstName &&
                ((InsertClientFileDocumentDataStatement)y).Surname == document.Surname &&
                ((InsertClientFileDocumentDataStatement)y).DataStoreGuid == dataStoreGuid &&
                ((InsertClientFileDocumentDataStatement)y).IdentityNumber == document.IdentityNumber &&
                ((InsertClientFileDocumentDataStatement)y).Category == document.Category &&
                ((InsertClientFileDocumentDataStatement)y).OriginalFileName == originalFileName)));
        };

        private It should_use_the_attorney_invoices_dataStor = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ISqlStatement<ClientFileDocumentModel>>.Matches(y =>
                ((InsertClientFileDocumentDataStatement)y).Store == storeID)));
        };

        private It should_save_it_as_a_pdf = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<ISqlStatement<ClientFileDocumentModel>>.Matches(y =>
                ((InsertClientFileDocumentDataStatement)y).Extension == FileExtension.Pdf.ToString())));
        };
    }
}