namespace DomainService2.Specs.Workflow.Life.DeclineQuoteCommandHandlerSpecs
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

    [Subject(typeof(DeclineQuoteCommandHandler))]
    public class When_a_life_application : DomainServiceSpec<DeclineQuoteCommand, DeclineQuoteCommandHandler>
    {
        static IApplicationRepository applicationRepository;
        static IAccountRepository accountRepository;
        static ICorrespondenceRepository correspondenceRepository;
        static ICommonRepository commonRepository;

        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
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

            command = new DeclineQuoteCommand(Param<int>.IsAnything);
            handler = new DeclineQuoteCommandHandler(applicationRepository, accountRepository, lookupRepository, reportRepository, correspondenceRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplicationLife>.IsAnything));
        };

        It should_update_account = () =>
        {
            accountRepository.WasToldTo(x => x.UpdateAccount(Param<int>.IsAnything, Param<int>.IsAnything, Param<float>.IsAnything, Param<string>.IsAnything));
        };

        It should_remove_correspondence = () =>
        {
            correspondenceRepository.WasToldTo(x => x.RemoveCorrespondenceByGenericKey(Param<int>.IsAnything, Param<int>.IsAnything, Param<bool>.IsAnything, Param<int>.IsAnything));
        };
    }
}