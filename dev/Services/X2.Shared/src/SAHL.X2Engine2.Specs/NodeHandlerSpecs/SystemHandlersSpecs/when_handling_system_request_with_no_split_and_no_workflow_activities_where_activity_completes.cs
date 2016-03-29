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
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.SystemHandlersSpecs
{
    public class when_handling_system_request_with_no_split_and_no_workflow_activities_where_activity_completes : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler> autoMocker;

        private static HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand command;
        private static string processName = "process";
        private static string workflowName = "workflow";
        private static string userName = "user";
        private static InstanceDataModel instance;
        private static Dictionary<string, string> mapVariables;
        private static IX2ContextualDataProvider contextualData;
        private static Activity activity;
        private static int fromStateId;
        private static int toStateId;
        private static long instanceId;
        private static IX2Map x2Map;
        private static IX2Process process;
        private static string stageTransitionComments;
        static WorkFlowDataModel workflow;
        static string alertMessage = "activityName";
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler>();

            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            instance = new InstanceDataModel(instanceId, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            mapVariables = new Dictionary<string, string>();
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(0, "activityName", fromStateId, "fromStataName", toStateId, "toState", 1, false);
            command = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity, userName);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceId)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(command.InstanceId)).Return(instance);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);

            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(stageTransitionComments);
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            x2Map.WhenToldTo(x => x.StartActivity(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(true);

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(
            x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>()), metadata)).
            Callback<HandleMapReturnCommand>((c) =>
            {
                c.Result = true;
            });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_the_instance_data = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceId));
        };

        private It should_get_the_contextual_data = () =>
        {
            x2Map.WasToldTo(x => x.GetContextualData(Param.IsAny<long>()));
        };

        private It should_remove_the_scheduled_activity = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<DeleteAllScheduleActivitiesCommand>(command => command.InstanceId == instanceId), metadata));
        };

        private It should_check_activity_is_valid_for_the_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(Param.IsAny<CheckActivityIsValidForStateCommand>(), metadata));
        };

        private It should_lock_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<LockInstanceCommand>(command => command.Instance.ID == instanceId), metadata));
        };

        private It should_get_the_map_for_the_case = () =>
        {
            process.WasToldTo(x => x.GetWorkflowMap(Param.IsAny<string>()));
        };

        private It should_load_contextual_data = () =>
        {
            contextualData.WasToldTo(x => x.LoadData(instanceId));
        };

        private It should_call_start_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.StartActivity(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_get_the_activity_message_from_the_map = () =>
        {
            x2Map.WasToldTo(x => x.GetActivityMessage(instance, contextualData, Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_call_complete_activity_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.CompleteActivity(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>(), ref alertMessage));
        };

        private It should_get_stage_transition_comments_from_the_map = () =>
        {
            x2Map.WasToldTo(x => x.GetStageTransition(instance, contextualData, Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_call_onexit_state_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.ExitState(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_update_the_instance_with_new_stateid = () =>
        {
            instance.StateID.ShouldEqual(activity.ToStateId);
        };

        private It should_call_onenter_state_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.EnterState(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_handle_any_archive_return_requests = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<WakeUpSourceInstanceAndPerformReturnActivityCommand>(command => command.Instance == instance && command.Activity == activity), metadata));
        };

        private It should_save_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(command => command.ContextualData == contextualData), metadata));
        };

        private It should_save_instance_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(command => command.Instance == instance), metadata));
        };

        private It should_save_a_workflow_history = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveWorkflowHistoryCommand>(
                command => command.Instance.ID == instance.ID &&
                    command.ToStateID == toStateId &&
                    command.ActivityID == activity.ActivityID), metadata));
        };

        private It should_save_a_new_stage_transition = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RecordStageTransitionCommand>(command => command.Instance == instance && command.StageTransitionComments == stageTransitionComments), metadata));
        };

        private It should_queue_up_any_system_activities = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<BuildSystemRequestToProcessCommand>(command => command.Instance == instance), metadata));
        };

        private It should_refresh_worklists = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshWorklistCommand>(command => command.Instance == instance && command.ContextualDataProvider == contextualData), metadata));
        };

        private It should_refresh_the_security_for_this_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshInstanceActivitySecurityCommand>(command => command.Instance == instance && command.ContextualDataProvider == contextualData), metadata));
        };

        private It should_unlock_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UnlockInstanceCommand>(command => command.InstanceID == instanceId), metadata));
        };

        private It should_return_true = () =>
        {
            command.Result.ShouldEqual(true);
        };
    }
}