using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.GetSystemRequestToProcessCommandHandlerSpecs
{
    public class when_getting_system_requests_to_process_where_there_are_workflow_activities : WithFakes
    {
        private static AutoMocker<BuildSystemRequestToProcessCommandHandler> automocker;
        private static BuildSystemRequestToProcessCommand command;
        private static InstanceDataModel instance;
        private static List<WorkFlowActivityDataModel> workflowActivities;
        private static StateDataModel state;
        private static Guid guid = Guid.NewGuid();
        private static WorkFlowActivityDataModel workflowActivityDataModel;
        private static IX2ContextualDataProvider contextualData;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                contextualData = An<IX2ContextualDataProvider>();
                automocker = new NSubstituteAutoMocker<BuildSystemRequestToProcessCommandHandler>();
                state = new StateDataModel(1, "AutoForwardState", 2, false, null, null, null, Guid.NewGuid());
                workflowActivities = new List<WorkFlowActivityDataModel>();
                workflowActivityDataModel = new WorkFlowActivityDataModel(1, "WorkflowActivity", 2, 99, 1, null);
                workflowActivities.Add(workflowActivityDataModel);
                instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
                command = new BuildSystemRequestToProcessCommand(instance, contextualData);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById((int)instance.StateID)).Return(state);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowActivitiesForState((int)instance.StateID)).Return(workflowActivities);
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_get_the_state_that_the_instance_is_at = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateById((int)instance.StateID));
        };

        private It should_get_all_activities_that_are_not_user_activities_that_come_off_this_state = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetSystemActivitiesForState((int)instance.StateID));
            };

        private It should_get_all_workflow_activities_that_come_off_this_state = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflowActivitiesForState((int)instance.StateID));
            };
    }
}