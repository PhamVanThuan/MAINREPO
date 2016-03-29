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
    public class When_credit_score_not_exists : DomainServiceSpec<GetCreditScoreDecisionKeyCommand, GetCreditScoreDecisionKeyCommandHandler>
    {
        protected static ICreditScoringRepository creditScoringRepository;

        Establish context = () =>
        {
            creditScoringRepository = An<ICreditScoringRepository>();

            IEventList<IApplicationCreditScore> lstCreditScore = new StubEventList<IApplicationCreditScore>();

            creditScoringRepository.WhenToldTo(x => x.GetApplicationCreditScoreByApplicationKey(Param.IsAny<int>())).Return(lstCreditScore);

            command = new GetCreditScoreDecisionKeyCommand(Param.IsAny<int>());
            handler = new GetCreditScoreDecisionKeyCommandHandler(creditScoringRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_minus_one = () =>
        {
            command.Result.ShouldEqual(-1);
        };
    }
}