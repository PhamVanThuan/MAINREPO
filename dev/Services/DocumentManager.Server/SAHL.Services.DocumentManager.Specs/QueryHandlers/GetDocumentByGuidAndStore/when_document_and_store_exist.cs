using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.Document.Models;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.QueryHandlers;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DocumentManager.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.QueryHandlers.GetDocumentByGuidAndStore
{
    public class when_document_and_store_exist : WithFakes
    {
        private static GetDocumentFromStorByDocumentGuidQueryHandler handler;
        private static GetDocumentFromStorByDocumentGuidQuery query;
        private static DocumentStoreModel documentStore;
        private static DocumentStorDocumentInfoModel documentInfo;
        private static ISystemMessageCollection messages;
        private static IDocumentDataManager documentDataManager;
        private static IDocumentFileManager documentFileManager;
        private static int storeId;
        private static string fileContent;

        Establish context = () =>
        {
            fileContent = "my content...==";
            documentDataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            handler = new GetDocumentFromStorByDocumentGuidQueryHandler(documentDataManager, documentFileManager);
            query = new GetDocumentFromStorByDocumentGuidQuery(DocumentStorEnum.LossControlAttorneyInvoices, Guid.Empty);
            storeId = Convert.ToInt32(query.Store);

            documentStore = new DocumentStoreModel(1402, "Document Store Name", "Document Store Description", "Folder", "", "default document title");
            documentInfo = new DocumentStorDocumentInfoModel { ArchiveDate = DateTime.Now, FileExtension = "pdf", FileName = "storeFileName.pdf" };

            documentDataManager.WhenToldTo(x => x.GetDocumentStore(storeId)).Return(new List<DocumentStoreModel> { documentStore });
            documentDataManager.WhenToldTo(x => x.GetDocumentInfoByGuidAndStoreId(query.DocumentGuid, storeId)).Return(documentInfo);
            documentFileManager.WhenToldTo(x => x.ReadFileFromDatedFolderAsBase64(query.DocumentGuid.ToString("B"), documentStore.Folder, documentInfo.ArchiveDate)).Return(fileContent);

        };

        Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        It should_retrieve_the_store = () =>
        {
            documentDataManager.WasToldTo(x => x.GetDocumentStore(storeId));
        };

        It should_retrieve_the_document_information = () =>
        {
            documentDataManager.WasToldTo(x => x.GetDocumentInfoByGuidAndStoreId(query.DocumentGuid, storeId));
        };

        It should_get_the_document_content_as_base64_string = () =>
        {
            documentFileManager.WasToldTo(x => x.ReadFileFromDatedFolderAsBase64(query.DocumentGuid.ToString("B"), documentStore.Folder, documentInfo.ArchiveDate));
        };

        It should_prepare_the_file_contents_and_details_into_the__query_result = () =>
        {
            query.Result.Results.Where(x => x.FileName == documentInfo.FileName
                                        && x.FileExtension == documentInfo.FileExtension
                                        && x.FileContentAsBase64 == fileContent)
                                        .FirstOrDefault()
                                        .ShouldNotBeNull();
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Any().ShouldBeFalse();
        };

    }
}
