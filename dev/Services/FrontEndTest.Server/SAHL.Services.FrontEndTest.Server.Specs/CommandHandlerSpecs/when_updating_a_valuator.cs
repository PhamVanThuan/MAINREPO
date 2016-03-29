using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_a_valuator : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdateValuatorCommand command;
        private static ValuatorDataModel valuator;
        private static int legalEntityKey;
        private static UpdateValuatorCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            legalEntityKey = 1235888;
            valuator = new ValuatorDataModel("valuatorContact", "valuatorPassword", 1, (int)GeneralStatus.Active, legalEntityKey);
            command = new UpdateValuatorCommand(valuator);
            testDataManager = An<ITestDataManager>();
            commandHandler = new UpdateValuatorCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_use_the_update_statement_through_the_testDataManager_to_update_the_variableLoanInfo = () =>
        {
            testDataManager.WasToldTo(x => x.UpdateValuator(valuator));
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}