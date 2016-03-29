namespace DomainService2.Specs.Workflow.Life.OlderThan45DaysCommandHandlerSpecs
{
    using System;
    using DomainService2.Specs.DomainObjects;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    [Subject(typeof(OlderThan45DaysCommandHandler))]
    public class When_life_application_open_complete_callback_success : DomainServiceSpec<OlderThan45DaysCommand, OlderThan45DaysCommandHandler>
    {
        static ILifeRepository lifeRepository;
        static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            applicationRepository = An<IApplicationRepository>();
            lifeRepository = An<ILifeRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IApplicationStatus applicationStatus = An<IApplicationStatus>();
            IEventList<IApplicationStatus> applicationStatuses = new StubEventList<IApplicationStatus>();
            IADUser aduser = An<IADUser>();
            ILegalEntityNaturalPerson le = An<ILegalEntityNaturalPerson>();
            IOriginationSourceProduct osp = An<IOriginationSourceProduct>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationStatus.WhenToldTo(x => x.Key).Return(Convert.ToInt32(SAHL.Common.Globals.OfferStatuses.Open));
            lifeApp.WhenToldTo(x => x.ApplicationStatus).Return(applicationStatus);
            lifeAcc.WhenToldTo(x => x.OriginationSourceProduct).Return(osp);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);
            lookupRepository.WhenToldTo(x => x.ApplicationStatuses).Return(applicationStatuses);
            applicationStatuses.ObjectDictionary.Add(Convert.ToString((int)OfferStatuses.Closed), null);
            applicationRepository.WhenToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything)).Return(true);
            lifeApp.WhenToldTo(x => x.Key).Return(Param<int>.IsAnything);
            le.WhenToldTo(x => x.EmailAddress).Return(Param<string>.IsAnything);
            aduser.WhenToldTo(x => x.LegalEntity).Return(le);
            lifeApp.WhenToldTo(x => x.Consultant).Return(aduser);

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

        It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplicationLife>.IsAnything));
        };

        It should_complete_callback = () =>
        {
            applicationRepository.WasToldTo(x => x.CompleteCallback(Param<int>.IsAnything, Param<DateTime>.IsAnything));
        };

        It should_send_internal_email = () =>
        {
            lifeRepository.WasToldTo(x => x.LifeApplicationArchivedWithCallBacks_SendInternalEmail(Param<int>.IsAnything, Param<int>.IsAnything, Param<long>.IsAnything, Param<string>.IsAnything));
        };

        It should_send_NTU_letter = () =>
        {
            lifeRepository.WasToldTo(x => x.LifeApplicationSendNTU_Letter(Param<int>.IsAnything, Param<int>.IsAnything));
        };
    }
}