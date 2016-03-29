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
using SAHL.Services.DocumentManager.Utils.PdfConverter;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.CommandHandlers.StoreClientFileDocumentsSpecs
{
    public class when_writing_client_documents_fails : WithFakes
    {
        private static StoreClientFileDocumentsCommandHandler handler;
        private static StoreClientFileDocumentsCommand command;

        private static IDocumentDataManager dataManager;
        private static IDocumentFileManager documentFileManager;
        private static IDataStoreUtils dataStoreUtils;
        private static IPdfConverter pdfConverter;
        private static ServiceRequestMetadata metadata;

        private static ISystemMessageCollection messages;
        private static DocumentStoreModel clientFilesStore;
        private static string dataStoreGuid;
        private static string username;
        private static ISystemMessageCollection fileManagerMessages;
        private static int storeId;

        private Establish context = () =>
        {
            dataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            dataStoreUtils = An<IDataStoreUtils>();
            pdfConverter = An<IPdfConverter>();
            metadata = new ServiceRequestMetadata();

            username = "SAHL\\Zorro";
            clientFilesStore = new DocumentStoreModel(17, "Client files", "Client files", "?!#;:<>-_=+()*&{}[]|,~", "\\\\sahl-ds01\\clientfiles$", "{K1}");
            var document = new ClientFileDocumentModel("1231456456456456", "Identity Documents", "1234567890132", "Bob", "Builder", username, 
                DateTime.Now, FileExtension.Pdf);
            var documentModels = new List<ClientFileDocumentModel> { document };
            dataStoreGuid = "{1234-4567-7894-4567}";
            fileManagerMessages = new SystemMessageCollection(new List<ISystemMessage> {
                new SystemMessage(@"Failed to write '1234567890132 - Identity Documents - 26-08-2014 10-36-40.pdf' to '\\\\sahl-ds01\\clientfiles$'. 
                    Please check file permissons.", SystemMessageSeverityEnum.Error) });

            command = new StoreClientFileDocumentsCommand(documentModels);
            handler = new StoreClientFileDocumentsCommandHandler(dataManager, documentFileManager, dataStoreUtils, pdfConverter, An<IUnitOfWorkFactory>());

            storeId = Convert.ToInt32(DocumentStorEnum.Clientfiles);
            dataManager.WhenToldTo(x => x.GetDocumentStore(storeId)).Return(new List<DocumentStoreModel> { clientFilesStore });
            dataStoreUtils.WhenToldTo(x => x.GenerateDataStoreGuid()).Return(dataStoreGuid);
            documentFileManager.WhenToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), dataStoreGuid, clientFilesStore.Folder, username, 
                Arg.Any<DateTime>())).Return(fileManagerMessages);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_details_about_the_client_files_store = () =>
        {
            dataManager.WasToldTo(x => x.GetDocumentStore(storeId));
        };

        private It should_attempt_to_write_the_document_to_the_specified_path = () =>
        {
            documentFileManager.WasToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), dataStoreGuid, clientFilesStore.Folder, username, Arg.Any<DateTime>()));
        };

        private It should_return_the_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(fileManagerMessages.ErrorMessages().First().Message);
        };
    }
}