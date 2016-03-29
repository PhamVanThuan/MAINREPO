using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Life;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Life.ActivateLifePolicyCommandHandlerSpecs
{
    [Subject(typeof(ActivateLifePolicyCommandHandler))]
    public class When_life_application_activate_life_policy : DomainServiceSpec<ActivateLifePolicyCommand, ActivateLifePolicyCommandHandler>
    {
        static ILifeRepository lifeRepository;
        static IApplicationRepository applicationRepository;
        static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            IApplicationLife lifeApp = An<IApplicationLife>();
            IAccount lifeAcc = An<IAccount>();
            applicationRepository = An<IApplicationRepository>();
            lifeRepository = An<ILifeRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IApplicationStatus applicationStatus = An<IApplicationStatus>();
            IEventList<IApplicationStatus> applicationStatuses = new StubEventList<IApplicationStatus>();
            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(lifeApp);
            applicationStatuses.ObjectDictionary.Add(Convert.ToString((int)OfferStatuses.Accepted), null);
            lookupRepository.WhenToldTo(x => x.ApplicationStatuses).Return(applicationStatuses);
            lifeAcc.WhenToldTo(x => x.Key).Return(Param<int>.IsAnything);
            lifeApp.WhenToldTo(x => x.Account).Return(lifeAcc);

            command = new ActivateLifePolicyCommand(Param<int>.IsAnything);
            handler = new ActivateLifePolicyCommandHandler(applicationRepository, lifeRepository, lookupRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplicationLife>.IsAnything));
        };

        It should_create_life_policy = () =>
        {
            lifeRepository.WasToldTo(x => x.CreateLifePolicy(Param<int>.IsAnything));
        };
    }
}