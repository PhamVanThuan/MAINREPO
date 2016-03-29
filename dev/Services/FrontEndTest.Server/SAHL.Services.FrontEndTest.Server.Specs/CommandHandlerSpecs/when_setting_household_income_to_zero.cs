using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_setting_household_income_to_zero : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static SetHouseholdIncomeToZeroCommandHandler commandHandler;
        private static SetHouseholdIncomeToZeroCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static int applicationNumber;

        private Establish context = () =>
         {
             testDataManager = An<ITestDataManager>();
             messages = An<ISystemMessageCollection>();
             metadata = An<IServiceRequestMetadata>();
             applicationNumber = 123546879;
             command = new SetHouseholdIncomeToZeroCommand(applicationNumber);
             commandHandler = new SetHouseholdIncomeToZeroCommandHandler(testDataManager);
         };

        private Because of = () =>
         {
             commandHandler.HandleCommand(command, metadata);
         };

        private It should = () =>
         {
             testDataManager.WasToldTo(x => x.UpdateHouseholdIncomeToZero(command.ApplicationNumber));
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