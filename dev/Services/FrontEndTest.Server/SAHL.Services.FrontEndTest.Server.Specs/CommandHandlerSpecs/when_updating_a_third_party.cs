using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_a_third_party : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdateThirdPartyCommand command;
        private static LegalEntityDataModel model;
        private static UpdateThirdPartyCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            model = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male, (int)PopulationGroup.Unknown, DateTime.Now, (int)SalutationType.Lord,
                                            "Vishav", "VP", "Premlall", "Vishav", "9209125163087", "", "", "", "Vishav", "", DateTime.Now, "", "031", "031", "0316605489", "0745847268",
                                            "vishaav.messi.premlall@gmail.com", "011", "", "", (int)CitizenType.SACitizen, (int)LegalEntityStatus.Alive, "", (int)LegalEntityExceptionStatus.Valid,
                                            "VishavP", DateTime.Now, (int)Education.UniversityDegree, 1, 1, (int)ResidenceStatus.Permanent);
            command = new UpdateThirdPartyCommand(model);
            testDataManager = An<ITestDataManager>();
            commandHandler = new UpdateThirdPartyCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_update_the_variableLoanInfo = () =>
        {
            testDataManager.WasToldTo(x => x.UpdateThirdParty(model));
        };

        private It should_return_messages = () =>
        {
            messages.AllMessages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}