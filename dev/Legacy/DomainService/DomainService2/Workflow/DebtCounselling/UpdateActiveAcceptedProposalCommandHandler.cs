using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.DebtCounselling
{
    public class UpdateActiveAcceptedProposalCommandHandler : IHandlesDomainServiceCommand<UpdateActiveAcceptedProposalCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;

        public UpdateActiveAcceptedProposalCommandHandler(IDebtCounsellingRepository debtcounsellingRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateActiveAcceptedProposalCommand command)
        {
            bool success = false;

            // get the active proposal record
            List<IProposal> activeProposals = debtcounsellingRepository.GetProposalsByTypeAndStatus(command.DebtCounsellingKey, ProposalTypes.Proposal, ProposalStatuses.Active);

            if (activeProposals == null || activeProposals.Count <= 0)
                throw new Exception("No Active Proposal exists for this Debt Counselling case.");
            else
            {
                activeProposals[0].Accepted = command.Accepted;
                debtcounsellingRepository.SaveProposal(activeProposals[0]);
                success = true;
            }

            command.Result = success;
        }
    }
}