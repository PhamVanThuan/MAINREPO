using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.SystemHandlersSpecs
{
    public class when_handling_system_request_with_no_split_with_workflow_activities : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommandHandler> autoMocker;

        private static HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand command;
        private static long instanceId = 0;
        private static long newlyCreatedInstance = 123;
        private static int returnActivityID = 999;
        private static int activityIDInDestinationMap = 111;
        private static IX2Map sourceMap;
        private static IX2Map destinationMap;
        private static string processName = "process";
        private static string sourceWorkflowName = "sourceWorkflow";
        private static string destWorfklowName = "destWorkflow";
        private static string userName = "user";
        private static string stateName = "HoldState";
        private static int stateId = 19;
        private static Activity activity;
        private static Activity returnActivity;
        private static X2Workflow sourceWorkflow;
        private static X2Workflow destinationWorkflow;
        private static InstanceDataModel instance;
        private static IX2Process process;
        private static WorkflowActivity workflowActivity;
        static ActivityDataModel destinationActivity;
        static IServiceRequestMetadata metadata;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommandHandler>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            returnActivity = new Activity(0, "CreateInDestinationMap", null, string.Empty, 1, "CreatedFromSourceMapState", 1, false);
            workflowActivity = new WorkflowActivity(1, sourceWorkflowName, destWorfklowName, activityIDInDestinationMap, returnActivityID, "workflowActivity");
            process = An<IX2Process>();
            sourceMap = An<IX2Map>();
            destinationMap = An<IX2Map>();
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(Param.IsAny<InstanceDataModel>())).Return(sourceWorkflowName);
            sourceWorkflow = new X2Workflow(processName, sourceWorkflowName);
            destinationWorkflow = new X2Workflow(processName, destWorfklowName);
            instance = new InstanceDataModel(instanceId, 1, null, "name", "subject", "workflowProvider", stateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, "activity", stateId, stateName, stateId, stateName, 1, false);
            destinationActivity = new ActivityDataModel(1, 2, "activity", 1, null, 2, false, 1, null, "message", null, null, null, "", null, Guid.NewGuid());

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivity(Param.IsAny<int>())).Return(destinationActivity);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(sourceWorkflow.WorkflowName)).Return(sourceMap);
            process.WhenToldTo(x => x.GetWorkflowMap(destinationWorkflow.WorkflowName)).Return(destinationMap);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessName(Param.IsAny<InstanceDataModel>())).Return(processName);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceId)).Return(instance);
            command = new HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand(instanceId, workflowActivity, userName, "workflowProviderName");
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowActivityById(command.WorkFlowActivity.ActivityIDInDestinationMap)).Return(workflowActivity);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivityById(returnActivityID)).Return(returnActivity);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<CreateInstanceCommand>(c => c.ProcessName == processName && c.WorkflowName == destWorfklowName), metadata)).
                Callback<CreateInstanceCommand>((c) =>
            {
                c.NewlyCreatedInstanceId = newlyCreatedInstance;
            });
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_check_activity_is_valid_for_the_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Param.IsAny<CheckActivityIsValidForStateCommand>(), metadata));
        };

        It should_get_a_map_for_the_source_workflow = () =>
        {
            process.WasToldTo(x => x.GetWorkflowMap(sourceWorkflowName));
        };

        It should_call_create_a_new_instance_with_the_sourceinstanceid_and_returnactivityid_set = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<UserRequestCreateInstanceCommand>(command =>
                command.ProcessName == processName
                && command.WorkflowName == destWorfklowName
                && command.SourceInstanceId == instanceId
                && command.ReturnActivityId == returnActivityID
                ), metadata));
        };

        It should_get_the_activity_to_complete_in_the_destination_map = () =>
            {
                autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivityById(activityIDInDestinationMap));
            };

        It should_complete_the_activity_in_the_destination_map_on_the_new_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<UserRequestCompleteActivityCommand>(), metadata));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldEqual(true);
        };
    }
}