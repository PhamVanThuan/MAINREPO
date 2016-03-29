using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.DocumentManager.Managers.Document.Models;
using SAHL.Services.DocumentManager.Managers.Document.Statements;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.Managers.Document
{
    public class when_getting_the_document_store : WithFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static DocumentDataManager documentDataManager;
        private static int storeId = 1;

        private Establish context = () =>
        {
            fakeDbFactory = An<FakeDbFactory>();
            documentDataManager = new DocumentDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            documentDataManager.GetDocumentStore(storeId);
        };

        private It should = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select<DocumentStoreModel>(Arg.Any<FindDocumentStoreByIdStatement>()));
        };
    }
}