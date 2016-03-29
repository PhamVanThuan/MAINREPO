using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class ConvertDebtCounsellingCommandHandler : IHandlesDomainServiceCommand<ConvertDebtCounsellingCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private ICommonRepository commonRepository;

        public ConvertDebtCounsellingCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, ICommonRepository commonRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, ConvertDebtCounsellingCommand command)
        {
            debtcounsellingRepository.ConvertDebtCounselling(command.AccountKey, command.UserID);
            commonRepository.RefreshDAOObject<IAccount>(command.AccountKey);
            command.Result = true;
        }
    }
}