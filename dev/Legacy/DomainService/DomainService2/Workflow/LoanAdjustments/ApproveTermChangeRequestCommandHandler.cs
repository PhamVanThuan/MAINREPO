namespace DomainService2.Workflow.LoanAdjustments
{
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class ApproveTermChangeRequestCommandHandler : IHandlesDomainServiceCommand<ApproveTermChangeRequestCommand>
    {
        private IAccountRepository AccountRepository;
        private IMortgageLoanRepository MortgageLoanRepository;
        private ICommonRepository CommonRepository;

        public ApproveTermChangeRequestCommandHandler(IAccountRepository accountRepository, IMortgageLoanRepository mortgageLoanRepository, ICommonRepository commonRepository)
        {
            this.AccountRepository = accountRepository;
            this.MortgageLoanRepository = mortgageLoanRepository;
            this.CommonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, ApproveTermChangeRequestCommand command)
        {
            IAccount acc = AccountRepository.GetAccountByKey(command.AccountKey);
            IMortgageLoanAccount mortgageLoanAccount = acc as IMortgageLoanAccount;
            IMortgageLoan vml = mortgageLoanAccount.SecuredMortgageLoan;

            int newTerm;

            MortgageLoanRepository.LookUpPendingTermChangeDetailFromX2(out newTerm, command.InstanceID);
            MortgageLoanRepository.TermChange(command.AccountKey, newTerm, "System");
            CommonRepository.RefreshDAOObject<IAccount>(command.AccountKey);

            command.Result = true;
        }
    }
}