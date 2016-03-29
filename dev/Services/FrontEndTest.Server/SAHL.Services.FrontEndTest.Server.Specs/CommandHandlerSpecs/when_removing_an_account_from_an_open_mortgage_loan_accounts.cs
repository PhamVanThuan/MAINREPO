using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_an_account_from_an_open_mortgage_loan_accounts : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static RemoveAccountFromOpenMortgageLoanAccountsCommand command;
        private static RemoveAccountFromOpenMortgageLoanAccountsCommandHandler commandHandler;
        private static int accountKey;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            testDataManager = An<ITestDataManager>();
            accountKey = 1235888;
            command = new RemoveAccountFromOpenMortgageLoanAccountsCommand(accountKey);
            commandHandler = new RemoveAccountFromOpenMortgageLoanAccountsCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_remove_the_account = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveOpenMortgageLoanAccount(accountKey));
        };
    }
}