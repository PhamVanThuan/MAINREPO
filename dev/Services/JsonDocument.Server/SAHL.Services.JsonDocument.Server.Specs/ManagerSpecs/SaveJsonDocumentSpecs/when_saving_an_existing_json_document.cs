using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.JsonDocument.Managers;
using SAHL.Services.JsonDocument.Managers.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.JsonDocument.Server.Specs.ManagerSpecs.SaveJsonDocumentSpecs
{
    public class when_saving_an_existing_json_document : WithFakes
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
            jsonDocDataManager.SaveJsonDocument(id, name, description, version, documentFormatVersion, documentType, data);
        };

        private It should_use_the_correct_statement = () =>
        {
            fakedDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update<JsonDocumentDataModel>(Arg.Any<UpdateJsonDocumentStatement>()));
        };

        private It should_update_the_document = () =>
        {
            fakedDbFactory.FakedDb.InAppContext().WasToldTo(x => x.Update<JsonDocumentDataModel>(Param<UpdateJsonDocumentStatement>.Matches(
                y => y.Id == id)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakedDbFactory.FakedDb.InAppContext().Received().Complete();
        };
    }
}
