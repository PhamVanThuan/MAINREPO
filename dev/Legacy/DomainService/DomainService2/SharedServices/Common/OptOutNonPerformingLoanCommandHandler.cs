using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class OptOutNonPerformingLoanCommandHandler : IHandlesDomainServiceCommand<OptOutNonPerformingLoanCommand>
    {
        private IAccountRepository accountRepository;
        private IFinancialServiceRepository financialServiceRepository;
        private ICommonRepository commonRepository;

        public OptOutNonPerformingLoanCommandHandler(IAccountRepository accountRepository, IFinancialServiceRepository financialServiceRepository, ICommonRepository commonRepository)
        {
            this.accountRepository = accountRepository;
            this.financialServiceRepository = financialServiceRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, OptOutNonPerformingLoanCommand command)
        {
            if (this.financialServiceRepository.IsLoanNonPerforming(command.AccountKey))
            {
                this.accountRepository.OptOutNonPerforming(command.AccountKey);
                commonRepository.RefreshDAOObject<IAccount>(command.AccountKey);
            }
        }
    }
}