using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateDebtCounsellingStatusCommandHandler : IHandlesDomainServiceCommand<UpdateDebtCounsellingStatusCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private ILookupRepository lookupRepository;

        public UpdateDebtCounsellingStatusCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, ILookupRepository lookupRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateDebtCounsellingStatusCommand command)
        {
            bool success = false;

            IDebtCounselling debtCounselling = debtcounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            if (debtCounselling != null)
            {
                debtCounselling.DebtCounsellingStatus = lookupRepository.DebtCounsellingStatuses[(DebtCounsellingStatuses)command.DebtCounsellingStatusKey];

                debtcounsellingRepository.SaveDebtCounselling(debtCounselling);

                success = true;
            }

            command.Result = success;
        }
    }
}