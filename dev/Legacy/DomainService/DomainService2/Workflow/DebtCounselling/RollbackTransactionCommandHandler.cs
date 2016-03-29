using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.Workflow.DebtCounselling
{
    public class RollbackTransactionCommandHandler : IHandlesDomainServiceCommand<RollbackTransactionCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private ICommonRepository commonRepository;

        public RollbackTransactionCommandHandler(IDebtCounsellingRepository debtcounsellingRepository,ICommonRepository commonRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, RollbackTransactionCommand command)
        {
            string msg = string.Empty;

            command.Result = debtcounsellingRepository.RollbackTransaction(command.DebtCounsellingKey);
            commonRepository.RefreshDAOObject<IDebtCounselling>(command.DebtCounsellingKey);

            if (command.Result == false)
                messages.Add(new Error(msg, msg));
        }
    }
}