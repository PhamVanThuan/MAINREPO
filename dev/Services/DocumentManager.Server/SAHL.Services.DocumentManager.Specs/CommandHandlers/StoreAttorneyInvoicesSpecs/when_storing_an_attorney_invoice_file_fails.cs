using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.CommandHandlers;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.Document.Models;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.DataStore;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.CommandHandlers.StoreAttorneyInvoicesSpecs
{
    public class when_storing_an_attorney_invoice_file_fails : WithFakes
    {
        private static StoreAttorneyInvoiceCommandHandler handler;
        private static StoreAttorneyInvoiceCommand command;

        private static IDocumentDataManager dataManager;
        private static IDocumentFileManager documentFileManager;
        private static IDataStoreUtils dataStoreUtils;
        private static ServiceRequestMetadata metadata;

        private static ISystemMessageCollection messages;
        private static ThirdPartyInvoiceDocumentModel invoice;
        private static DocumentStoreModel documentStore;
        private static string dataStoreGuid;
        private static ISystemMessageCollection fileManagerMessages;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            dataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            dataStoreUtils = An<IDataStoreUtils>();
            var uowFactory = An<IUnitOfWorkFactory>();

            metadata = new ServiceRequestMetadata();
            invoice = new ThirdPartyInvoiceDocumentModel("12345", DateTime.Now, DateTime.Now, "test@some.email.com", "12345 - Some Invoice", "INV3-Some Invoice", "pdf", "Category",
                "Some Contetnt");
            dataStoreGuid = "{123-456-789-123-456}";
            thirdPartyInvoiceKey = 839742;

            command = new StoreAttorneyInvoiceCommand(invoice, thirdPartyInvoiceKey.ToString());
            handler = new StoreAttorneyInvoiceCommandHandler(dataManager, documentFileManager, dataStoreUtils, uowFactory);

            documentStore = new DocumentStoreModel(command.StoreId, "Loss Control - Attorney Invoices", "Loss Control - Attorney Invoices", "?!#;:<>-_=+()*&{}[]|,~", 
                "\\\\sahl-ds01\\attorneyinvoices$", "{K4}");
            fileManagerMessages = new SystemMessageCollection(new List<ISystemMessage> {
                new SystemMessage("Failed to write '1234567890132 - Identity Documents - 26-08-2014 10-36-40.pdf' to '\\\\sahl-ds01\\clientfiles$'. Please check file permissons.", 
                    SystemMessageSeverityEnum.Error) });

            dataManager.WhenToldTo(x => x.GetDocumentStore(Arg.Any<int>())).Return(new List<DocumentStoreModel> { documentStore });
            dataStoreUtils.WhenToldTo(x => x.GenerateDataStoreGuid()).Return(dataStoreGuid);
            documentFileManager.WhenToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), dataStoreGuid, documentStore.Folder, metadata.UserName, Arg.Any<DateTime>())).Return(fileManagerMessages);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_details_about_the_document_store = () =>
        {
            dataManager.WasToldTo(x => x.GetDocumentStore(Arg.Any<int>()));
        };

        private It should_attempt_to_write_the_document_to_the_specified_path = () =>
        {
            documentFileManager.WasToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), dataStoreGuid, documentStore.Folder, metadata.UserName, Arg.Any<DateTime>()));
        };

        private It should_return_the_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(fileManagerMessages.ErrorMessages().First().Message);
        };
    }
}