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

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_valuation : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static ValuatorDataModel valuator;
        private static string valuatorContact;
        private static int legalEntityKey;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static UpdateValuatorCommandHandler commandHandler;
        private static UpdateValuatorCommand command;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            messages = An<ISystemMessageCollection>();

            legalEntityKey = 123;
            valuatorContact = "VishavP";
            valuator = new ValuatorDataModel(valuatorContact, "!@#$%^&*()", 1, (int)GeneralStatus.Active, legalEntityKey);
            command = new UpdateValuatorCommand(valuator);
            commandHandler = new UpdateValuatorCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_update_the_correct_valuator_with_the_correct_details = () =>
        {
            testDataManager.WasToldTo(x=>x.UpdateValuator(valuator));
        };

        private It should_return_messages = () =>
         {
             messages.ShouldNotBeNull();
         };
    }
}