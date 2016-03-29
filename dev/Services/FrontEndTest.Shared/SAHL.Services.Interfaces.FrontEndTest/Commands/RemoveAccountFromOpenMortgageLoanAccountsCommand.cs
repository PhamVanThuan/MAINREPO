using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveAccountFromOpenMortgageLoanAccountsCommand : ServiceCommand, IFrontEndTestCommand
    {
        public RemoveAccountFromOpenMortgageLoanAccountsCommand(int accountNumber)
        {
            this.AccountNumber = accountNumber;
        }

        public int AccountNumber { get; protected set; }
    }
}