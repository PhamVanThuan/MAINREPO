using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class ProcessDebtCounsellingOptOutCommandHandler : IHandlesDomainServiceCommand<ProcessDebtCounsellingOptOutCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private ICommonRepository commonRepository;

        public ProcessDebtCounsellingOptOutCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, ICommonRepository commonRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, ProcessDebtCounsellingOptOutCommand command)
        {
            debtcounsellingRepository.ProcessDebtCounsellingOptOut(command.AccountKey, command.UserID);
            commonRepository.RefreshDAOObject<IAccount>(command.AccountKey);
            command.Result = true;
        }
    }
}