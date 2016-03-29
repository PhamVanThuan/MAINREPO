namespace DomainService2.Specs.Workflow.Life.PolicyNTUdCommandHandlerSpecs
{
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(PolicyNTUdCommandHandler))]
    public class When_life_application_closed : DomainServiceSpec<PolicyNTUdCommand, PolicyNTUdCommandHandler>
    {
        static ILifeRepository lifeRepository;

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            lifeRepository = An<ILifeRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);

            command = new PolicyNTUdCommand(Param<int>.IsAnything);
            handler = new PolicyNTUdCommandHandler(applicationRepository, lifeRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_close_life_application = () =>
        {
            lifeRepository.WasToldTo(x => x.CloseLifeApplication(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything));
        };
    }
}