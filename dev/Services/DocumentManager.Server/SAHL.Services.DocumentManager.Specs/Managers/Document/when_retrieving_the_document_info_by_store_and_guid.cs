using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.Document.Statements;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;

namespace SAHL.Services.DocumentManager.Specs.Managers.Document
{
    public class when_retrieving_the_document_info_by_store_and_guid : WithFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static DocumentDataManager documentDataManager;
        private static int storeId = 1;
        private static Guid documentGuid;
        private static DocumentStorDocumentInfoModel documentInfoModel;

        private Establish context = () =>
        {
            documentGuid = Guid.NewGuid();
            fakeDbFactory = An<FakeDbFactory>();
            documentDataManager = new DocumentDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            documentInfoModel = documentDataManager.GetDocumentInfoByGuidAndStoreId(documentGuid, storeId);
        };

        private It should_create_a_readonly_db_context = () =>
        {
            fakeDbFactory.WasToldTo(x => x.NewDb().InReadOnlyAppContext());
        };

        private It should_select_the_document_info_from_datastore = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<DocumentStorDocumentInfoModel>(
                Param<FindDocumentInformationByDocumentIdAndStoreIdStatement>.Matches(
                    y => y.DocumentGuid == documentGuid.ToString("B") && y.StoreId == storeId)));
        };
    }
}