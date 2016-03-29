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

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.UserHandlerSpecs
{
    public class when_handling_user_action_complete : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<UserRequestCompleteActivityCommandHandler> autoMocker;

        private static UserRequestCompleteActivityCommand userRequestCompleteActivityCommand;
        private static long instanceID = 0;
        private static Activity activity;
        private static InstanceDataModel instance;
        private static IX2ContextualDataProvider contextualData;
        private static IX2Map x2Map;
        private static Dictionary<string, string> mapVariables;
        private static int toStateId = 5;
        private static string userWhoPerformedTheActivity = "user";
        private static int? fromStateId = 1;
        private static string stageTransitionComments = "comments";
        private static IX2Process process;
        static string alertMessage = "activityName";
        static WorkFlowDataModel workflow;
        static IServiceRequestMetadata metadata;
        static ProcessDataModel legacyProcessDataModel;

        private Establish context = () =>
            {
                autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<UserRequestCompleteActivityCommandHandler>();

                x2Map = An<IX2Map>();
                process = An<IX2Process>();
                metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
                instance = new InstanceDataModel(instanceID, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
                legacyProcessDataModel = new ProcessDataModel(1, "name", "version", null, DateTime.Now, null, null, null, true);
                mapVariables = new Dictionary<string, string>();
                contextualData = An<IX2ContextualDataProvider>();
                activity = new Activity(0, "activityName", fromStateId, "fromStataName", toStateId, "toState", 1, false);
                userRequestCompleteActivityCommand = new UserRequestCompleteActivityCommand(instanceID, activity, userWhoPerformedTheActivity, false, mapVariables);
                autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceID)).Return(instance);
                autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(userRequestCompleteActivityCommand.InstanceId)).Return(instance);
                autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(Param.IsAny<int>())).Return(legacyProcessDataModel);
                autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
                process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);
                x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(stageTransitionComments);
                x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
                workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
                autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);

                autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>()), metadata)).Callback<HandleMapReturnCommand>(y => { y.Result = true; });
            };

        private Because of = () =>
            {
                autoMocker.ClassUnderTest.HandleCommand(userRequestCompleteActivityCommand, metadata);
            };

        private It should_get_instance_data = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceID));
        };

        private It should_check_the_activity_is_valid_for_the_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(Param.IsAny<CheckActivityIsValidForStateCommand>(), metadata));
        };

        private It should_check_the_instance_is_locked_for_this_user = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(Param.IsAny<CheckInstanceIsLockedForUserCommand>(), metadata));
        };

        private It should_get_contextual_data = () =>
        {
            x2Map.WasToldTo(x => x.GetContextualData(Param.IsAny<long>()));
        };

        private It should_set_contextual_data = () =>
        {
            contextualData.WasToldTo(x => x.SetMapVariables(mapVariables));
        };

        private It should_get_the_map_for_the_case = () =>
        {
            process.WasToldTo(x => x.GetWorkflowMap(Param.IsAny<string>()));
        };

        private It should_get_the_activity_message_from_the_map = () =>
        {
            x2Map.WasToldTo(x => x.GetActivityMessage(instance, contextualData, Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_call_complete_activity_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.CompleteActivity(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>(), ref alertMessage));
        };

        It should_check_the_result_of_the_map_call = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Param.IsAny<HandleMapReturnCommand>(), metadata)).Times(3);
            };

        private It should_call_onexit_state_on_the_map = () =>
        {
            x2Map.WasToldTo(x => x.ExitState(instance, contextualData, Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
        };

        private It should_update_the_instance_with_new_stateid = () =>
        {
            instance.StateID.ShouldEqual(activity.ToStateId);
        };

        private It should_queue_up_and_external_activities = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<QueueUpExternalActivitiesCommand>(command => command.Instance == instance && command.Activity == activity), metadata));
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

        private It should_get_stage_transition_comments_from_the_map = () =>
        {
            x2Map.WasToldTo(x => x.GetStageTransition(instance, contextualData, Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        private It should_save_a_new_stage_transition = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RecordStageTransitionCommand>(command => command.Instance == instance && command.StageTransitionComments == stageTransitionComments), metadata));
        };
    }
}