using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.LoanAdjustments
{
    public class CheckIfCanApproveTermChangeRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckIfCanApproveTermChangeRulesCommand>
    {
        protected IAccountRepository AccountRepository;

        public CheckIfCanApproveTermChangeRulesCommandHandler(ICommandHandler commandHandler, IAccountRepository accountRepository)
            : base(commandHandler)
        {
            this.AccountRepository = accountRepository;
        }

        public override void SetupRule()
        {
            IAccount acc = AccountRepository.GetAccountByKey(command.AccountKey);
            IMortgageLoanAccount mortgageLoanAccount = acc as IMortgageLoanAccount;
            IMortgageLoan vml = mortgageLoanAccount.SecuredMortgageLoan;
            command.RuleParameters = new object[] { vml, command.InstanceID };
        }
    }
}