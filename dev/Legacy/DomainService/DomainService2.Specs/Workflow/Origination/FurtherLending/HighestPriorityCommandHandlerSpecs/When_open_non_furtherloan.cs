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

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.HighestPriorityCommandHandlerSpecs
{
    [Subject(typeof(HighestPriorityCommandHandler))]
    public class When_open_non_furtherloan : WithFakes
    {
        static HighestPriorityCommand command;
        static HighestPriorityCommandHandler handler;
        static IDomainMessageCollection messages;

        Establish context = () =>
        {
            IApplication app = An<IApplication>();
            IApplication currentapp = An<IApplication>();
            IApplicationType appType = An<IApplicationType>();
            IApplicationType currentappType = An<IApplicationType>();
            IApplicationStatus currentappStatus = An<IApplicationStatus>();
            IAccount account = An<IAccount>();
            IInstance instance = An<IInstance>();
            IState state = An<IState>();
            IWorkFlow workflow = An<IWorkFlow>();
            IEventList<IApplication> applications = new StubEventList<IApplication>();

            IX2Repository x2Repository = An<IX2Repository>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);

            app.WhenToldTo(x => x.Account).Return(account);
            app.WhenToldTo(x => x.ApplicationType).Return(appType);
            appType.WhenToldTo(x => x.Key).Return((int)OfferTypes.ReAdvance);

            account.WhenToldTo(x => x.Applications).Return(applications);

            messages = new DomainMessageCollection();

            applications.Add(messages, currentapp);

            currentapp.WhenToldTo(x => x.Key).Return(1);
            currentapp.WhenToldTo(x => x.ApplicationType).Return(currentappType);
            currentappType.WhenToldTo(x => x.Key).Return((int)OfferTypes.Life);
            currentapp.WhenToldTo(x => x.ApplicationStatus).Return(currentappStatus);
            currentappStatus.WhenToldTo(x => x.Key).Return((int)OfferStatuses.Open);

            x2Repository.WhenToldTo(x => x.GetInstanceForGenericKey(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(instance);
            instance.WhenToldTo(x => x.ID).Return(1);
            instance.WhenToldTo(x => x.State).Return(state);
            state.WhenToldTo(x => x.Name).Return(WorkflowState.QuickCashHold);

            x2Repository.WhenToldTo(x => x.GetWorkFlowByName(Param.IsAny<string>(), Param.IsAny<string>())).Return(workflow);
            x2Repository.WhenToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.Is<string>(Constants.WorkFlowExternalActivity.MoveApplicationToHold),
                                                                                Param.IsAny<long>(), Param.IsAny<string>(),
                                                                                Param.IsAny<string>(), Param.IsAny<string>()));
            command = new HighestPriorityCommand(1);
            handler = new HighestPriorityCommandHandler(applicationRepository, x2Repository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It Should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}