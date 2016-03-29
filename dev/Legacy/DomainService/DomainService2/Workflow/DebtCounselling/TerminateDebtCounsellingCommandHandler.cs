using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using X2DomainService.Interface.Common;

namespace DomainService2.Workflow.DebtCounselling
{
    public class TerminateDebtCounsellingCommandHandler : IHandlesDomainServiceCommand<TerminateDebtCounsellingCommand>
    {
        private IDebtCounsellingRepository debtCounsellingRepository;
        private ICommon commonWorkflowService;
        private ICommonRepository commonRepository;

        public TerminateDebtCounsellingCommandHandler(IDebtCounsellingRepository debtCounsellingRepository, ICommon commonWorkflowService, ICommonRepository commonRepository)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
            this.commonWorkflowService = commonWorkflowService;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, TerminateDebtCounsellingCommand command)
        {
            // get the debtcounselling record
            var debtCounselling = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
            if (debtCounselling == null)
                throw new Exception("No DebtCounselling record exists.");

            // this will update the debtcounselling status and call out-out sp if there is an accepted proposal
            debtCounsellingRepository.CancelDebtCounselling(debtCounselling, command.UserID, DebtCounsellingStatuses.Terminated);
            commonRepository.RefreshDAOObject<IDebtCounselling>(debtCounselling.Key);
            commonRepository.RefreshDAOObject<IAccount>(debtCounselling.Account.Key);

            command.Result = true;
        }
    }
}