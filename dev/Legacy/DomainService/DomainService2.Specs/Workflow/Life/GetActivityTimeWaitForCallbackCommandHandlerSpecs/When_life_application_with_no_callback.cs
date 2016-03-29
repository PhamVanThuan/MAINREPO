namespace DomainService2.Specs.Workflow.Life.GetActivityTimeWaitForCallbackCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(GetActivityTimeWaitForCallbackCommandHandler))]
    public class When_life_application_with_no_callback : DomainServiceSpec<GetActivityTimeWaitForCallbackCommand, GetActivityTimeWaitForCallbackCommandHandler>
    {
        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            ICallback callBack = null;
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationRepository.WhenToldTo(x => x.GetLatestCallBackByApplicationKey(Param<int>.IsAnything, Param<bool>.IsAnything)).Return(callBack);

            command = new GetActivityTimeWaitForCallbackCommand(Param<int>.IsAnything);
            handler = new GetActivityTimeWaitForCallbackCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.ActivityTimeResult.ShouldBeCloseTo(DateTime.Now, new TimeSpan(10000000));
        };
    }
}