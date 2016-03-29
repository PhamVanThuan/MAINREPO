using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Life;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Life.ReactivatePolicyCommandHandlerSpecs
{
    [Subject(typeof(ReactivatePolicyCommandHandler))]
    public class When_a_life_application : DomainServiceSpec<ReactivatePolicyCommand, ReactivatePolicyCommandHandler>
    {
        static IApplicationRepository applicationRepository;
        static IAccountRepository accountRepository;
        static ILifeRepository lifeRepository;
        static ICorrespondenceRepository correspondenceRepository;
        static ICommonRepository commonRepository;

        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccountLifePolicy lifePolicyAcc = An<IAccountLifePolicy>();
            IOriginationSourceProduct osp = An<IOriginationSourceProduct>();
            IReportStatement reportStatement = An<IReportStatement>();
            applicationRepository = An<IApplicationRepository>();
            accountRepository = An<IAccountRepository>();
            correspondenceRepository = An<ICorrespondenceRepository>();
            lifeRepository = An<ILifeRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IReportRepository reportRepository = An<IReportRepository>();
            IEventList<IApplicationStatus> applicationStatuses = new StubEventList<IApplicationStatus>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationStatuses.ObjectDictionary.Add(Convert.ToString((int)OfferStatuses.Open), null);
            lookupRepository.WhenToldTo(x => x.ApplicationStatuses).Return(applicationStatuses);
            lifePolicyAcc.WhenToldTo(x => x.OriginationSourceProduct).Return(osp);
            lifeApp.WhenToldTo(x => x.Account).Return(lifePolicyAcc);
            reportRepository.WhenToldTo(x => x.GetReportStatementByNameAndOSP(Param<string>.IsAnything, Param<int>.IsAnything)).Return(reportStatement);

            command = new ReactivatePolicyCommand(Param<int>.IsAnything);
            handler = new ReactivatePolicyCommandHandler(applicationRepository, accountRepository, lifeRepository, lookupRepository, reportRepository, correspondenceRepository, commonRepository);
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

        It should_recalculate_premium = () =>
        {
            lifeRepository.WasToldTo(x => x.RecalculateSALifePremium(Param<IAccountLifePolicy>.IsAnything, Param<bool>.IsAnything));
        };

        It should_remove_correspondence = () =>
        {
            correspondenceRepository.WasToldTo(x => x.RemoveCorrespondenceByReportStatementAndGenericKey(Param<int>.IsAnything, Param<int>.IsAnything, Param<int>.IsAnything, Param<bool>.IsAnything));
        };
    }
}