using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_a_disability_claim : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static int DisabilityClaimKey;
        private static RemoveDisabilityClaimCommand command;
        private static RemoveDisabilityClaimCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            DisabilityClaimKey = 123;
            command = new RemoveDisabilityClaimCommand(DisabilityClaimKey);
            commandHandler = new RemoveDisabilityClaimCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_remove_the_correct_disability_claim = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveDisabilityClaimRecord(DisabilityClaimKey));
        };

        private It should_remove_the_correct_disability_payment_record = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveDisabilityPaymentRecord(DisabilityClaimKey));
        };
    }
}