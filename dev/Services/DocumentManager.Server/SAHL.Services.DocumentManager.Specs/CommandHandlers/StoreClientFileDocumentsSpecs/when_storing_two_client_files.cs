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

namespace SAHL.Services.DocumentManager.Specs.CommandHandlers.StoreClientFileDocumentsSpecs
{
    public class when_storing_two_client_files : WithFakes
    {
        private static StoreClientFileDocumentsCommandHandler handler;
        private static StoreClientFileDocumentsCommand command;

        private static IDocumentDataManager dataManager;
        private static IDocumentFileManager documentFileManager;
        private static IDataStoreUtils dataStoreUtils;
        private static ServiceRequestMetadata metadata;

        private static ISystemMessageCollection messages;
        private static ClientFileDocumentModel firstDocument;
        private static ClientFileDocumentModel secondDocument;
        private static string firstOriginalFileName;
        private static string secondOriginalFileName;
        private static DocumentStoreModel clientFilesStore;
        private static int storeId;

        private Establish context = () =>
        {
            dataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            dataStoreUtils = An<IDataStoreUtils>();
            clientFilesStore = new DocumentStoreModel(17, "Client files", "Client files", "?!#;:<>-_=+()*&{}[]|,~", "\\\\sahl-ds01\\clientfiles$", "{K1}");
            metadata = new ServiceRequestMetadata();
            firstDocument = new ClientFileDocumentModel("QQBsAGwAJwBzACAAdwBlAGwAbAAgAHQAaABhAHQAIABlAG4AZABzACAAdwBlAGwAbAAuAA==", "Identity Documents", "1234567890132", "Bob", "Builder", "Zorro", 
                DateTime.Now, FileExtension.Pdf);
            secondDocument = new ClientFileDocumentModel("QQBsAGwAJwBzACAAdwBlAGwAbAAgAHQAaABhAHQAIABlAG4AZABzACAAdwBlAGwAbAAuAA==", "Deed of Sale", "1234567890132", "Bob", "Builder", "Zorro", 
                DateTime.Now, FileExtension.Pdf);
            var documentModels = new List<ClientFileDocumentModel> { firstDocument, secondDocument };
            firstOriginalFileName = "1234567890132 - Identity Documents - 26-08-2014 10-36-40";
            secondOriginalFileName = "1234567890132 - Deed of Sale - 26-08-2014 10-36-40";
            command = new StoreClientFileDocumentsCommand(documentModels);
            handler = new StoreClientFileDocumentsCommandHandler(dataManager, documentFileManager, dataStoreUtils, An<IPdfConverter>(), An<IUnitOfWorkFactory>());

            storeId = Convert.ToInt32(DocumentStorEnum.Clientfiles);
            dataManager.WhenToldTo(x => x.GetDocumentStore(storeId)).Return(new List<DocumentStoreModel> { clientFilesStore });
            dataStoreUtils.WhenToldTo(x => x.GetFileNameForClientFileDocument(firstDocument, Arg.Any<DateTime>())).Return(firstOriginalFileName);
            dataStoreUtils.WhenToldTo(x => x.GetFileNameForClientFileDocument(secondDocument, Arg.Any<DateTime>())).Return(secondOriginalFileName);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_details_about_the_client_files_store = () =>
        {
            dataManager.WasToldTo(x => x.GetDocumentStore(storeId));
        };

        private It should_get_filenames_for_each_document = () =>
        {
            dataStoreUtils.WasToldTo(x => x.GetFileNameForClientFileDocument(firstDocument, Arg.Any<DateTime>()));
            dataStoreUtils.WasToldTo(x => x.GetFileNameForClientFileDocument(secondDocument, Arg.Any<DateTime>()));
        };

        private It should_write_both_documents_to_disk = () =>
        {
            documentFileManager.WasToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), Arg.Any<string>(), clientFilesStore.Folder, Arg.Any<string>(), Arg.Any<DateTime>())).Times(2);
        };

        private It should_save_each_document_to_client_files_store = () =>
        {
            dataManager.WasToldTo(x => x.SaveClientFile(firstDocument, Arg.Any<string>(), firstOriginalFileName, Arg.Any<DateTime>()));
            dataManager.WasToldTo(x => x.SaveClientFile(secondDocument, Arg.Any<string>(), secondOriginalFileName, Arg.Any<DateTime>()));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}