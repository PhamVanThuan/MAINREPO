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
    public class when_storing_a_pdf_client_file : WithFakes
    {
        private static StoreClientFileDocumentsCommandHandler handler;
        private static StoreClientFileDocumentsCommand command;

        private static IDocumentDataManager dataManager;
        private static IDocumentFileManager documentFileManager;
        private static IDataStoreUtils dataStoreUtils;
        private static IPdfConverter pdfConverter;
        private static ServiceRequestMetadata metadata;

        private static ISystemMessageCollection messages;
        private static ClientFileDocumentModel document;
        private static DocumentStoreModel clientFilesStore;
        private static string originalFileName;
        private static string dataStoreGuid;
        private static int storeId;

        private Establish context = () =>
        {
            dataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            dataStoreUtils = An<IDataStoreUtils>();
            pdfConverter = An<IPdfConverter>();
            var uowFactory = An<IUnitOfWorkFactory>();

            clientFilesStore = new DocumentStoreModel(17, "Client files", "Client files", "?!#;:<>-_=+()*&{}[]|,~", "\\\\sahl-ds01\\clientfiles$", "{K1}");
            metadata = new ServiceRequestMetadata();
            document = new ClientFileDocumentModel("QQBsAGwAJwBzACAAdwBlAGwAbAAgAHQAaABhAHQAIABlAG4AZABzACAAdwBlAGwAbAAuAA==", "Identity Documents", "1234567890132", "Bob", "Builder",
                "Zorro", DateTime.Now, FileExtension.Pdf);
            var documentModels = new List<ClientFileDocumentModel> { document };
            originalFileName = "1234567890132 - Identity Documents - 26-08-2014 10-36-40";
            dataStoreGuid = "{123-456-789-123-456}";
            command = new StoreClientFileDocumentsCommand(documentModels);
            handler = new StoreClientFileDocumentsCommandHandler(dataManager, documentFileManager, dataStoreUtils, pdfConverter, uowFactory);

            storeId = Convert.ToInt32(DocumentStorEnum.Clientfiles);
            dataManager.WhenToldTo(x => x.GetDocumentStore(storeId)).Return(new List<DocumentStoreModel> { clientFilesStore });
            dataStoreUtils.WhenToldTo(x => x.GetFileNameForClientFileDocument(document, Arg.Any<DateTime>())).Return(originalFileName);
            dataStoreUtils.WhenToldTo(x => x.GenerateDataStoreGuid()).Return(dataStoreGuid);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_get_the_details_about_the_client_files_store = () =>
        {
            dataManager.WasToldTo(x => x.GetDocumentStore(storeId));
        };

        private It should_get_the_correct_file_name_for_the_document = () =>
        {
            dataStoreUtils.WasToldTo(x => x.GetFileNameForClientFileDocument(document, Arg.Any<DateTime>()));
        };

        private It should_generate_a_data_store_guid_for_the_document = () =>
        {
            dataStoreUtils.WasToldTo(x => x.GenerateDataStoreGuid());
        };

        private It should_not_convert_the_pdf = () =>
        {
            pdfConverter.WasNotToldTo(x => x.ConvertImageToPdf(Arg.Any<byte[]>(), 17));
        };

        private It should_write_the_document_to_the_specified_path = () =>
        {
            documentFileManager.WasToldTo(x => x.WriteFileToDatedFolder(Arg.Any<byte[]>(), dataStoreGuid, clientFilesStore.Folder, document.Username, Arg.Any<DateTime>()));
        };

        private It should_save_the_document_to_client_files_store = () =>
        {
            dataManager.WasToldTo(x => x.SaveClientFile(document, dataStoreGuid, originalFileName, Arg.Any<DateTime>()));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}