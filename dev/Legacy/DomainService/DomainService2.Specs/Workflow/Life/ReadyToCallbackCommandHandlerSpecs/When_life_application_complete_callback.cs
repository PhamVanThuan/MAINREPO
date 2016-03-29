namespace DomainService2.Specs.Workflow.Life.ReadyToCallbackCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(ReadyToCallbackCommandHandler))]
    public class When_life_application_complete_callback : DomainServiceSpec<ReadyToCallbackCommand, ReadyToCallbackCommandHandler>
    {
        static ILifeRepository lifeRepository;
        static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            applicationRepository = An<IApplicationRepository>();
            lifeRepository = An<ILifeRepository>();
            IADUser aduser = An<IADUser>();
            ILegalEntityNaturalPerson le = An<ILegalEntityNaturalPerson>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);
            applicationRepository.WhenToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything)).Return(true);
            le.WhenToldTo(x => x.EmailAddress).Return("not an empty string");
            aduser.WhenToldTo(x => x.LegalEntity).Return(le);
            lifeApp.WhenToldTo(x => x.Consultant).Return(aduser);

            command = new ReadyToCallbackCommand(Param<int>.IsAnything, Param<long>.IsAnything);
            handler = new ReadyToCallbackCommandHandler(applicationRepository, lifeRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_complete_callback = () =>
        {
            applicationRepository.WasToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything));
        };

        It should_send_internal_email = () =>
        {
            lifeRepository.WasToldTo(x => x.LifeApplicationReadyToCallback_SendInternalEmail(Param<int>.IsAnything, Param<int>.IsAnything, Param<long>.IsAnything, Param<string>.IsAnything));
        };
    }
}