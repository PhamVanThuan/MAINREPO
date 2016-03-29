namespace DomainService2.Specs.Workflow.Life.NTUPolicyCommandHandlerSpecs
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

    [Subject(typeof(NTUPolicyCommandHandler))]
    public class When_a_life_application : DomainServiceSpec<NTUPolicyCommand, NTUPolicyCommandHandler>
    {
        static IApplicationRepository applicationRepository;
        static IAccountRepository accountRepository;
        static ICorrespondenceRepository correspondenceRepository;

        Establish context = () =>
        {
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            IOriginationSourceProduct osp = An<IOriginationSourceProduct>();
            IReportStatement reportStatement = An<IReportStatement>();
            applicationRepository = An<IApplicationRepository>();
            accountRepository = An<IAccountRepository>();
            correspondenceRepository = An<ICorrespondenceRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IReportRepository reportRepository = An<IReportRepository>();
            IEventList<IApplicationStatus> applicationStatuses = new StubEventList<IApplicationStatus>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationStatuses.ObjectDictionary.Add(Convert.ToString((int)OfferStatuses.NTU), null);
            lookupRepository.WhenToldTo(x => x.ApplicationStatuses).Return(applicationStatuses);
            lifeAcc.WhenToldTo(x => x.OriginationSourceProduct).Return(osp);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);
            reportRepository.WhenToldTo(x => x.GetReportStatementByNameAndOSP(Param<string>.IsAnything, Param<int>.IsAnything)).Return(reportStatement);

            command = new NTUPolicyCommand(Param<int>.IsAnything);
            handler = new NTUPolicyCommandHandler(applicationRepository, accountRepository, lookupRepository, reportRepository, correspondenceRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplicationLife>.IsAnything));
        };

        It should_remove_correspondence = () =>
        {
            correspondenceRepository.WasToldTo(x => x.RemoveCorrespondenceByGenericKey(Param<int>.IsAnything, Param<int>.IsAnything, Param<bool>.IsAnything, Param<int>.IsAnything));
        };
    }
}