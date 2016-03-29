using DomainService2.SharedServices.Common;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.GetCreditScoreDecisionKeyCommandHandlerSpecs
{
    [Subject(typeof(GetCreditScoreDecisionKeyCommandHandler))]
    public class When_credit_score_exists : DomainServiceSpec<GetCreditScoreDecisionKeyCommand, GetCreditScoreDecisionKeyCommandHandler>
    {
        protected static ICreditScoringRepository creditScoringRepository;

        Establish context = () =>
            {
                creditScoringRepository = An<ICreditScoringRepository>();

                IEventList<IApplicationCreditScore> lstCreditScore = new StubEventList<IApplicationCreditScore>();
                IApplicationCreditScore appCreditScore = An<IApplicationCreditScore>();
                IApplicationAggregateDecision appAggregateDecision = An<IApplicationAggregateDecision>();
                ICreditScoreDecision creditScoreDecision = An<ICreditScoreDecision>();

                appCreditScore.WhenToldTo(x => x.ApplicationAggregateDecision).Return(appAggregateDecision);
                appAggregateDecision.WhenToldTo(x => x.CreditScoreDecision).Return(creditScoreDecision);
                creditScoreDecision.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());

                lstCreditScore.Add(null, appCreditScore);

                creditScoringRepository.WhenToldTo(x => x.GetApplicationCreditScoreByApplicationKey(Param.IsAny<int>())).Return(lstCreditScore);

                command = new GetCreditScoreDecisionKeyCommand(Param.IsAny<int>());
                handler = new GetCreditScoreDecisionKeyCommandHandler(creditScoringRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_greater_than_minus_one = () =>
            {
                command.Result.ShouldBeGreaterThan(-1);
            };
    }
}