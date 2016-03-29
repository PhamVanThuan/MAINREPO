using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_inserting_a_client : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static InsertClientCommand command;
        private static InsertClientCommandHandler commandHandler;
        private static LegalEntityDataModel legalEntityDataModel;

        private Establish context = () =>
            {
                testDataManager = An<ITestDataManager>();
                uowFactory = An<IUnitOfWorkFactory>();
                linkedKeyManager = An<ILinkedKeyManager>();
                metadata = An<IServiceRequestMetadata>();
                legalEntityDataModel = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male, (int)PopulationGroup.Unknown, DateTime.Now, (int)SalutationType.Lord,
                                            "Vishav", "VP", "Premlall", "Vishav", "9209125163087", "", "", "", "Vishav", "", DateTime.Now, "", "031", "031", "0316605489", "0745847268",
                                            "vishaav.messi.premlall@gmail.com", "011", "", "", (int)CitizenType.SACitizen, (int)LegalEntityStatus.Alive, "", (int)LegalEntityExceptionStatus.Valid,
                                            "VishavP", DateTime.Now, (int)Education.UniversityDegree, 1, 1, (int)ResidenceStatus.Permanent);
                command = new InsertClientCommand(legalEntityDataModel, 1, Guid.NewGuid());
                commandHandler = new InsertClientCommandHandler(testDataManager, uowFactory, linkedKeyManager);
            };

        private Because of = () =>
            {
                messages = commandHandler.HandleCommand(command, metadata);
            };

        private It should_return_messages = () =>
            {
                messages.ShouldNotBeNull();
            };

        private It should_insert_the_client = () =>
            {
                testDataManager.WasToldTo(x => x.InsertClient(legalEntityDataModel));
            };

        private It should_not_return_error_messages = () =>
            {
                messages.HasErrors.ShouldBeFalse();
            };
    }
}