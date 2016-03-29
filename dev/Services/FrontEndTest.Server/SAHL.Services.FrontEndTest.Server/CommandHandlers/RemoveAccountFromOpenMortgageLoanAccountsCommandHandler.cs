using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveAccountFromOpenMortgageLoanAccountsCommandHandler : IServiceCommandHandler<RemoveAccountFromOpenMortgageLoanAccountsCommand>
    {
        private ITestDataManager testDataManager;

        public RemoveAccountFromOpenMortgageLoanAccountsCommandHandler(ITestDataManager testDataManager)
        {
            this.testDataManager = testDataManager;
        }

        public ISystemMessageCollection HandleCommand(RemoveAccountFromOpenMortgageLoanAccountsCommand command, IServiceRequestMetadata metadata)
        {
            this.testDataManager.RemoveOpenMortgageLoanAccount(command.AccountNumber);
            return SystemMessageCollection.Empty();
        }
    }
}