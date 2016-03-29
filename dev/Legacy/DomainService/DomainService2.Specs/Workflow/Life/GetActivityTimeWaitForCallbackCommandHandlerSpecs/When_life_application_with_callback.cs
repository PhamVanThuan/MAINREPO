namespace DomainService2.Specs.Workflow.Life.GetActivityTimeWaitForCallbackCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(GetActivityTimeWaitForCallbackCommandHandler))]
    public class When_life_application_with_callback : DomainServiceSpec<GetActivityTimeWaitForCallbackCommand, GetActivityTimeWaitForCallbackCommandHandler>
    {
        static DateTime callbackDate = DateTime.Now.AddDays(2);

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            ICallback callBack = An<ICallback>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationRepository.WhenToldTo(x => x.GetLatestCallBackByApplicationKey(Param<int>.IsAnything, Param<bool>.IsAnything)).Return(callBack);
            callBack.WhenToldTo(x => x.CallbackDate).Return(callbackDate);

            command = new GetActivityTimeWaitForCallbackCommand(Param<int>.IsAnything);
            handler = new GetActivityTimeWaitForCallbackCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.ActivityTimeResult.ShouldEqual<DateTime>(callbackDate);
        };
    }
}