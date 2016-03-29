namespace DomainService2.Specs.Workflow.Life.ContinueSaleCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(ContinueSaleCommandHandler))]
    public class When_life_application_complete_callback : DomainServiceSpec<ContinueSaleCommand, ContinueSaleCommandHandler>
    {
        static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            IApplicationLife lifeApp = An<IApplicationLife>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationRepository.WhenToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything)).Return(true);

            command = new ContinueSaleCommand(Param<int>.IsAnything);
            handler = new ContinueSaleCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_complete_callback = () =>
        {
            applicationRepository.WasToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything));
        };
    }
}