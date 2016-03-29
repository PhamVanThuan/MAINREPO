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
    public class when_store_does_not_exist : WithFakes
    {
        private static GetDocumentFromStorByDocumentGuidQueryHandler handler;
        private static GetDocumentFromStorByDocumentGuidQuery query;
        private static DocumentStoreModel documentStore;
        private static ISystemMessageCollection messages;
        private static IDocumentDataManager documentDataManager;
        private static IDocumentFileManager documentFileManager;
        private static int storeId;

        private Establish context = () =>
        {
            documentDataManager = An<IDocumentDataManager>();
            documentFileManager = An<IDocumentFileManager>();
            handler = new GetDocumentFromStorByDocumentGuidQueryHandler(documentDataManager, documentFileManager);
            query = new GetDocumentFromStorByDocumentGuidQuery(DocumentStorEnum.LossControlAttorneyInvoices, Guid.Empty);
            storeId = Convert.ToInt32(query.Store);

            documentStore = null;
            documentDataManager.WhenToldTo(x => x.GetDocumentStore(storeId)).Return(new List<DocumentStoreModel> { documentStore });
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_retrieve_the_store = () =>
        {
            documentDataManager.WasToldTo(x => x.GetDocumentStore(storeId));
        };

        private It should_not_retrieve_the_document_information = () =>
        {
            documentDataManager.WasNotToldTo(x => x.GetDocumentInfoByGuidAndStoreId(query.DocumentGuid, storeId));
        };

        private It should_not_get_the_document_content_as_base64_string = () =>
        {
            documentFileManager.WasNotToldTo(x => x.ReadFileFromDatedFolderAsBase64(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<DateTime>()));
        };

        private It should_prepare_an_empty_query_result = () =>
        {
            query.Result.Results.First().FileName.ShouldBeNull();
            query.Result.Results.First().FileExtension.ShouldBeNull();
            query.Result.Results.First().FileContentAsBase64.ShouldBeNull();
        };

        private It should_return_an_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}