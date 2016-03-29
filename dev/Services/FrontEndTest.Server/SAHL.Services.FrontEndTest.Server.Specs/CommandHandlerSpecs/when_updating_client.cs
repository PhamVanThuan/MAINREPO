using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_client : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdateClientCommandHandler updateClientCommandHandler;
        private static UpdateClientCommand updateClientCommand;
        private static ISystemMessageCollection expectedMessages;
        private static FakeDbFactory fakeDb;
        private static LegalEntityDataModel legalEntityDataModel;
        private static IServiceRequestMetadata metadata;

        private Establish Context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = An<ITestDataManager>();
            legalEntityDataModel = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male, (int)PopulationGroup.Unknown, DateTime.Now, (int)SalutationType.Lord,
                                            "Vishav", "VP", "Premlall", "Vishav", "9209125163087", "", "", "", "Vishav", "", DateTime.Now, "", "031", "031", "0316605489", "0745847268",
                                            "vishaav.messi.premlall@gmail.com", "011", "", "", (int)CitizenType.SACitizen, (int)LegalEntityStatus.Alive, "", (int)LegalEntityExceptionStatus.Valid,
                                            "VishavP", DateTime.Now, (int)Education.UniversityDegree, 1, 1, (int)ResidenceStatus.Permanent);
            updateClientCommand = new UpdateClientCommand(legalEntityDataModel);
            updateClientCommandHandler = new UpdateClientCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
            expectedMessages = An<ISystemMessageCollection>();
        };

        private Because of = () =>
        {
            expectedMessages = updateClientCommandHandler.HandleCommand(updateClientCommand, metadata);
        };

        private It should_update_the_client = () =>
        {
            testDataManager.WasToldTo(x => x.UpdateClient(legalEntityDataModel));
        };

        private It should_return_messages = () =>
        {
            expectedMessages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            expectedMessages.HasErrors.ShouldBeFalse();
        };
    }
}