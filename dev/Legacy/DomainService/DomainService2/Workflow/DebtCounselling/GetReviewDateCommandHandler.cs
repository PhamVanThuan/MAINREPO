using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetReviewDateCommandHandler : IHandlesDomainServiceCommand<GetReviewDateCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;

        public GetReviewDateCommandHandler(IDebtCounsellingRepository debtcounsellingRepository)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetReviewDateCommand command)
        {
            IDebtCounselling debtCounselling = debtcounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            if (debtCounselling.AcceptedActiveProposal == null)
                throw new Exception("No Active Proposal exists for this Debt Counselling case.");
            else if (!debtCounselling.AcceptedActiveProposal.ReviewDate.HasValue)
                throw new Exception("There is no Review Date for this Debt Counselling case.");
            else
            {
                command.ReviewDateResult = debtCounselling.AcceptedActiveProposal.ReviewDate;
            }
        }
    }
}