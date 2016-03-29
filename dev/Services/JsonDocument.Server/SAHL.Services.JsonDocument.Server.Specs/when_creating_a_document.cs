using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.JsonDocument.Commands;
using SAHL.Services.JsonDocument.CommandHandlers;
using SAHL.Services.JsonDocument.Managers;
using System;

namespace SAHL.Services.JsonDocument.Server.Specs
{
    public class when_creating_a_document : WithFakes
    {
        private static string jsonDoc;
        private static CreateOrUpdateDocumentCommand saveOrCreateDocumentCommand;
        private static CreateOrUpdateDocumentCommandHandler handler;
        private static IJsonDocumentDataManager jsonDocumentDataManager;
        private static IServiceRequestMetadata metadata = null;

        private static Guid expectedId;
        private static string expectedName;
        private static string expectedDescription;
        private static string expectedDocumentFormatVersion;
        private static string expectedDocumentType;
        private static string expectedData;
        private static int expectedVersion;



        private Establish context = () =>
        {
            expectedId = Guid.Parse("f70df161-e59a-416d-93d0-a4bd00ccdf73");
            expectedName = "Bob";
            expectedDescription = "this is bobs profile";
            expectedDocumentFormatVersion = "2.2";
            expectedDocumentType = "UserProfile";
            expectedData = "{data}";
            expectedVersion = 1;

            jsonDoc = @"{
                            ""id"":""f70df161-e59a-416d-93d0-a4bd00ccdf73"",
                            ""name"":""Bob"",
                            ""description"":""this is bobs profile"",
                            ""version"":0,
                            ""documentType"":""UserProfile"",
                            ""documentFormatVersion"":""2.2"",
                            ""document"":""{data}""
                      }";

            jsonDocumentDataManager = An<IJsonDocumentDataManager>();
            jsonDocumentDataManager.WhenToldTo(x => x.DoesDocumentExist(expectedId)).Return(false);
            saveOrCreateDocumentCommand = new CreateOrUpdateDocumentCommand(jsonDoc);

            handler = new CreateOrUpdateDocumentCommandHandler(jsonDocumentDataManager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(saveOrCreateDocumentCommand, metadata);
        };

        private It should_parse_the_json_and_call_the_data_manager_to_save_the_document = () =>
        {
            jsonDocumentDataManager.WasToldTo(x => x.CreateJsonDocument(expectedId, expectedName, expectedDescription, expectedVersion, expectedDocumentFormatVersion, expectedDocumentType, expectedData));
        };
    }
}