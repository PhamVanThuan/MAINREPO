using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.JsonDocument.Managers;
using SAHL.Services.JsonDocument.Managers.Statements;
using System;
using System.Linq;

namespace SAHL.Services.JsonDocument.Server.Specs.ManagerSpecs.CreateJsonDocumentSpecs
{
    public class when_creating_a_json_document : WithFakes
    {
        private static FakeDbFactory fakedDbFactory;
        private static JsonDocumentDataManager jsonDocDataManager;
        private static Guid id;
        private static int version;
        private static string name, description, documentFormatVersion, documentType, data;

        private Establish context = () =>
            {
                id = Guid.NewGuid();
                version = 1;
                name = "documentName";
                description = "documentDescription";
                documentFormatVersion = "5";
                documentType = "Test";
                data = "data";
                fakedDbFactory = new FakeDbFactory();
                jsonDocDataManager = new JsonDocumentDataManager(fakedDbFactory);
            };

        private Because of = () =>
            {
                jsonDocDataManager.CreateJsonDocument(id, name, description, version, documentFormatVersion, documentType, data);
            };

        private It should_use_the_correct_statement = () =>
            {
                fakedDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<JsonDocumentDataModel>(Arg.Any<InsertJsonDocumentStatement>()));
            };

        private It should_update_using_the_document_details_provided = () =>
            {
                fakedDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<JsonDocumentDataModel>(Param<InsertJsonDocumentStatement>.Matches(
                    y => y.Id == id)));
            };

        private It should_complete_the_db_context = () =>
            {
                fakedDbFactory.FakedDb.InAppContext().Received().Complete();
            };
    }
}