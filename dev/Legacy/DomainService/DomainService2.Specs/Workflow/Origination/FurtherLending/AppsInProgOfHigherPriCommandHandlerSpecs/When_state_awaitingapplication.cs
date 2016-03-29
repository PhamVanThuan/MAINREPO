using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.AppsInProgOfHigherPriCommandHandlerSpecs
{
    [Subject(typeof(AppsInProgOfHigherPriCommandHandler))]
    public class When_state_awaitingapplication : WithFakes
    {
        static AppsInProgOfHigherPriCommand command;
        static AppsInProgOfHigherPriCommandHandler handler;
        static IDomainMessageCollection messages;

        Establish context = () =>
            {
                IApplication app = An<IApplication>();
                IApplicationType appType = An<IApplicationType>();

                IApplication currentApp = An<IApplication>();
                IApplicationType currentAppType = An<IApplicationType>();
                IApplicationStatus currentAppStatus = An<IApplicationStatus>();
                IInstance instance = An<IInstance>();
                IState state = An<IState>();

                IAccount account = An<IAccount>();
                IEventList<IApplication> applications = new StubEventList<IApplication>();

                IApplicationRepository applicationRepository = An<IApplicationRepository>();
                IX2Repository x2Repository = An<IX2Repository>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);
                app.WhenToldTo(x => x.ApplicationType).Return(appType);
                app.WhenToldTo(x => x.Account).Return(account);

                appType.WhenToldTo(x => x.Key).Return((int)OfferTypes.ReAdvance);
                account.WhenToldTo(x => x.Applications).Return(applications);

                messages = new DomainMessageCollection();

                applications.Add(messages, currentApp);

                currentApp.WhenToldTo(x => x.ApplicationType).Return(currentAppType);
                currentAppType.WhenToldTo(x => x.Key).Return((int)OfferTypes.FurtherAdvance);

                currentApp.WhenToldTo(x => x.ApplicationStatus).Return(currentAppStatus);
                currentAppStatus.WhenToldTo(x => x.Key).Return((int)OfferStatuses.Open);

                x2Repository.WhenToldTo(x => x.GetInstanceForGenericKey(Param.IsAny<int>(), Param.IsAny<string>(),
                                                                        Param.IsAny<string>())).Return(instance);

                instance.WhenToldTo(x => x.State).Return(state);
                state.WhenToldTo(x => x.Name).Return(WorkflowState.AwaitingApplication);

                command = new AppsInProgOfHigherPriCommand(Param.IsAny<int>());
                handler = new AppsInProgOfHigherPriCommandHandler(applicationRepository, x2Repository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_false = () =>
            {
                command.Result.ShouldBeFalse();
            };
    }
}