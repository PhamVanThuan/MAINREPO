using System;
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
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.SystemHandlersSpecs
{
    public class when_handling_system_request_with_a_split_and_no_workflow_activities : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommandHandler> autoMocker;

        private static HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand command;
        private static InstanceDataModel instance;
        private static InstanceDataModel newlyCreatedInstance;
        private static Activity activity;
        private static IX2Map map;
        private static IX2Process process;
        private static IX2ContextualDataProvider newlyCreatedContextualData;
        private static string stageTransitionComments = "STC", userName = "userName";
        static WorkFlowDataModel workflow;
        static string alertMessage = "GotoHold";
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommandHandler>();

            map = An<IX2Map>();
            process = An<IX2Process>();
            newlyCreatedContextualData = An<IX2ContextualDataProvider>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            newlyCreatedInstance = new InstanceDataModel(10, 1, 9, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, "GotoHold", 10, "state", 11, "HoldState", 1, false);
            command = new HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand(instance.ID, activity, userName, "workflowProviderName");

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instance.ID)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(newlyCreatedInstance.ID)).Return(newlyCreatedInstance);
            map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(newlyCreatedContextualData);
            map.WhenToldTo(x => x.GetStageTransition(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(stageTransitionComments);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);

            autoMocker.Get<IX2ServiceCommandRouter>()
                .WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<CreateChildInstanceCommand>(c => c.Instance == instance), metadata)).Callback<CreateChildInstanceCommand>((c) =>
            {
                c.CreatedInstance = newlyCreatedInstance;
            });

            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>()), metadata)).Callback<HandleMapReturnCommand>(y => { y.Result = true; });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_the_instance_for_the_source_instance = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instance.ID));
        };

        private It should_check_activity_is_valid_for_the_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Param.IsAny<CheckActivityIsValidForStateCommand>(), metadata));
        };

        private It should_remove_the_split_scheduled_activity = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<DeleteScheduledActivityCommand>(c => c.InstanceId == instance.ID), metadata));
        };

        // Do the split
        private It should_create_a_child_instance = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CreateChildInstanceCommand>(c => c.Instance.ID == instance.ID), metadata));
            };

        private It should_load_the_instance_record_for_the_new_child_instance = () =>
            {
                autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(newlyCreatedInstance.ID));
            };

        private It should_create_a_contextual_data_for_the_child_using_the_childs_data = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<CreateContextualDataCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
            };

        private It should_load_contextual_data_for_the_newly_created_instance = () =>
            {
                map.GetContextualData(newlyCreatedInstance.ID);
            };

        private It should_call_onstart_on_the_new_instance = () =>
            {
                map.WasToldTo(x => x.StartActivity(newlyCreatedInstance, Param.IsAny<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
            };

        private It should_call_getactivitymessage_on_the_new_instance = () =>
            {
                map.WasToldTo(x => x.GetActivityMessage(newlyCreatedInstance, Param.IsAny<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
            };

        private It should_call_completeactivity_on_the_new_instance = () =>
            {
                map.WasToldTo(x => x.CompleteActivity(newlyCreatedInstance, Param.IsAny<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>(), ref alertMessage));
            };

        private It should_getstagetransitioncomments_on_the_new_instance = () =>
            {
                map.WasToldTo(x => x.ExitState(newlyCreatedInstance, Param.IsAny<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>()));
            };

        private It should_update_the_new_instace_with_the_new_state_info = () =>
            {
                newlyCreatedInstance.StateID.ShouldEqual(activity.ToStateId);
            };

        private It should_queue_external_activities = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<QueueUpExternalActivitiesCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
            };

        private It should_call_enterstate_on_the_new_instance = () =>
            {
                map.EnterState(newlyCreatedInstance, Param.IsAny<IX2ContextualDataProvider>(), Arg.Any<IX2Params>(), Arg.Any<ISystemMessageCollection>());
            };

        private It should_handle_archive_return_requests = () =>
            {
                autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<WakeUpSourceInstanceAndPerformReturnActivityCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
            };

        private It should_save_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Param.IsAny<SaveContextualDataCommand>(), metadata));
        };

        private It should_save_the_newly_created_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
        };

        private It should_save_a_workflow_history = () =>
  {
      autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveWorkflowHistoryCommand>(
          c => c.Instance.ID == newlyCreatedInstance.ID &&
              c.ToStateID == activity.ToStateId &&
              c.ActivityID == activity.ActivityID), metadata));
  };

        private It should_save_a_new_stage_transition = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RecordStageTransitionCommand>(c => c.Instance.ID == newlyCreatedInstance.ID
                && c.StageTransitionComments == stageTransitionComments), metadata));
        };

        private It should_queue_up_any_system_activities = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<BuildSystemRequestToProcessCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
        };

        private It should_refresh_worklists = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshWorklistCommand>(c => c.Instance.ID == newlyCreatedInstance.ID), metadata));
        };

        private It should_refresh_the_security_for_this_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshInstanceActivitySecurityCommand>(c => c.Instance.ID == newlyCreatedInstance.ID
                && c.ContextualDataProvider == newlyCreatedContextualData), metadata));
        };

        private It should_return_true = () =>
        {
            command.Result.ShouldEqual(true);
        };
    }
}