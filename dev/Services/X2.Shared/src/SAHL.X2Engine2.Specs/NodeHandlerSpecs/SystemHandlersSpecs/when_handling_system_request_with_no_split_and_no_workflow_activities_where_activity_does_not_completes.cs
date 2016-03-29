using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.SystemHandlersSpecs
{
    public class when_handling_system_request_with_no_split_and_no_workflow_activities_where_activity_does_not_completes : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler> autoMocker;

        private static HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand systemRequestWithNoSplitNoWorkflowActivitiesCommand;
        private static string processName = "process";
        private static string workflowName = "workflow";
        private static string userName = "user";
        private static InstanceDataModel instance;
        private static Dictionary<string, string> mapVariables;
        private static IX2ContextualDataProvider contextualData;
        private static Activity activity;
        private static int fromStateId = 1;
        private static int toStateId = 2;
        private static long instanceId = 5;
        private static IX2Map x2Map;
        private static IX2Process process;
        private static string stageTransitionComments = "comments";
        static WorkFlowDataModel workflow;
        static Exception exception;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler>();

            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            instance = new InstanceDataModel(instanceId, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            mapVariables = new Dictionary<string, string>();
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(0, "activityName", fromStateId, "fromStataName", toStateId, "toState", 1, false);
            systemRequestWithNoSplitNoWorkflowActivitiesCommand = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity, userName);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceId)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(systemRequestWithNoSplitNoWorkflowActivitiesCommand.InstanceId)).Return(instance);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);
            x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(stageTransitionComments);
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            x2Map.WhenToldTo(x => x.StartActivity(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(false);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo(x => x.HandleCommand<HandleMapReturnCommand>(Param.IsAny<HandleMapReturnCommand>(), metadata)).Throw(new MapReturnedFalseException(null));
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => autoMocker.ClassUnderTest.HandleCommand(systemRequestWithNoSplitNoWorkflowActivitiesCommand, metadata));
        };

        private It should_get_the_instance_data = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceId));
        };

        private It should_get_the_contextual_data = () =>
        {
            x2Map.WasToldTo(x => x.GetContextualData(Param.IsAny<long>()));
        };

        private It should_check_activity_is_valid_for_the_state = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.CheckRuleCommand(
                Param.IsAny<CheckActivityIsValidForStateCommand>(), metadata));
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

        private It should_throw_an_Exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}