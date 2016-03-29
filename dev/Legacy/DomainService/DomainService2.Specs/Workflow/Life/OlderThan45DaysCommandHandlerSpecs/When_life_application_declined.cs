namespace DomainService2.Specs.Workflow.Life.OlderThan45DaysCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(OlderThan45DaysCommandHandler))]
    public class When_life_application_declined : DomainServiceSpec<OlderThan45DaysCommand, OlderThan45DaysCommandHandler>
    {
        static ILifeRepository lifeRepository;

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            lifeRepository = An<ILifeRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IApplicationStatus applicationStatus = An<IApplicationStatus>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationStatus.WhenToldTo(x => x.Key).Return(Convert.ToInt32(SAHL.Common.Globals.OfferStatuses.Declined));
            lifeApp.WhenToldTo(x => x.ApplicationStatus).Return(applicationStatus);
            lifeAcc.WhenToldTo(x => x.Key).Return(Param<int>.IsAnything);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);

            command = new OlderThan45DaysCommand(Param<int>.IsAnything, Param<long>.IsAnything);
            handler = new OlderThan45DaysCommandHandler(applicationRepository, lifeRepository, lookupRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_close_life_application = () =>
        {
            lifeRepository.WasToldTo(x => x.CloseLifeApplication(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything)).OnlyOnce();
        };
    }
}