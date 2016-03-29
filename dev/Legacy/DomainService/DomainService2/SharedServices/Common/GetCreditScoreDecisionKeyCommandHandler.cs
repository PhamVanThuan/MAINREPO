using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetCreditScoreDecisionKeyCommandHandler : IHandlesDomainServiceCommand<GetCreditScoreDecisionKeyCommand>
    {
        private ICreditScoringRepository creditScoringRepository;

        public GetCreditScoreDecisionKeyCommandHandler(ICreditScoringRepository creditScoringRepository)
        {
            this.creditScoringRepository = creditScoringRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetCreditScoreDecisionKeyCommand command)
        {
            IEventList<IApplicationCreditScore> lstCreditScore = creditScoringRepository.GetApplicationCreditScoreByApplicationKey(command.ApplicationKey);
            if (lstCreditScore.Count > 0)
            {
                command.Result = lstCreditScore[lstCreditScore.Count - 1].ApplicationAggregateDecision.CreditScoreDecision.Key;
            }
            else
            {
                command.Result = -1;
            }
        }
    }
}