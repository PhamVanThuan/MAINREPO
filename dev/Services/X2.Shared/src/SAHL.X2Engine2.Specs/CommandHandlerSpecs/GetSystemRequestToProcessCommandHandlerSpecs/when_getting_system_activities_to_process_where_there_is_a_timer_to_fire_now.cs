using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.GetSystemRequestToProcessCommandHandlerSpecs
{
    public class when_getting_system_activities_to_process_where_there_is_a_timer_to_fire_now : WithFakes
    {
        private static AutoMocker<BuildSystemRequestToProcessCommandHandler> automocker;
        private static BuildSystemRequestToProcessCommand command;
        private static InstanceDataModel instance;
        private static StateDataModel state;
        static WorkFlowDataModel workflow;
        private static Guid guid = Guid.NewGuid();
        private static List<ActivityDataModel> systemActivities;
        private static ActivityDataModel activityDataModel;
        private static IX2Map map;
        private static IX2ProcessProvider processProvider;
        private static IX2Process process;
        private static IX2ContextualDataProvider contextualData;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
                contextualData = An<IX2ContextualDataProvider>();
                map = An<IX2Map>();
                map.WhenToldTo(x => x.GetActivityTime(instance, Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(DateTime.Now.AddDays(-1));

                process = An<IX2Process>();
                process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);

                processProvider = An<IX2ProcessProvider>();
                processProvider.WhenToldTo(x => x.GetProcessForInstance(Param.IsAny<long>())).Return(process);

                automocker = new NSubstituteAutoMocker<BuildSystemRequestToProcessCommandHandler>();
                automocker.Inject<IX2ProcessProvider>(processProvider);
                state = new StateDataModel(1, "SomeState", 2, false, null, null, null, Guid.NewGuid());

                systemActivities = new List<ActivityDataModel>();
                activityDataModel = new ActivityDataModel(1, "timer", 4, 1, 2, false, 1, null, "", null, null, null, null, null, guid);
                systemActivities.Add(activityDataModel);

                command = new BuildSystemRequestToProcessCommand(instance, contextualData);
                workflow = new WorkFlowDataModel(1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById((int)instance.StateID)).Return(state);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSystemActivitiesForState((int)instance.StateID)).Return(systemActivities);
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

        private It should_get_a_workflow_process = () =>
            {
                processProvider.GetProcessForInstance(command.Instance.ID);
            };

        private It should_get_the_time_to_execute_from_the_map = () =>
            {
                map.GetActivityTime(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>());
            };

        private It should_insert_a_scheduled_activity = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<InsertScheduledActivityCommand>(c => c.ActivityDataModel.ID == activityDataModel.ID), metadata));
        };

        private It should_queue_up_a_commmand_to_get_sent_to_the_engine_to_inform_it_there_are_new_scheduled_activities_to_load = () =>
            {
                automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.QueueUpCommandToBeProcessed(Arg.Is<PublishBundledRequestCommand>(y => ((NotificationOfNewScheduledActivityCommand)y.Commands[0]).ActivityId == activityDataModel.ID)));
            };
    }
}