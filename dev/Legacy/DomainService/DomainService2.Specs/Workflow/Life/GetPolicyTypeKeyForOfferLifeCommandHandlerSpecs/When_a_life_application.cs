using DomainService2.Workflow.Life;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Life.GetPolicyTypeKeyForOfferLifeHandlerSpecs
{
    [Subject(typeof(GetPolicyTypeKeyForOfferLifeCommandHandler))]
    public class When_a_life_application : DomainServiceSpec<GetPolicyTypeKeyForOfferLifeCommand, GetPolicyTypeKeyForOfferLifeCommandHandler>
    {
        static IApplicationLife lifeApp;

        Establish context = () =>
        {
            lifeApp = An<IApplicationLife>();
            ILifePolicyType lifePolicyType = An<ILifePolicyType>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            lifeApp.WhenToldTo(x => x.LifePolicyType).Return(lifePolicyType);

            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);

            command = new GetPolicyTypeKeyForOfferLifeCommand(Param<int>.IsAnything);
            handler = new GetPolicyTypeKeyForOfferLifeCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.PolicyTypeKeyResult.ShouldBeGreaterThanOrEqualTo(0);
        };
    }
}