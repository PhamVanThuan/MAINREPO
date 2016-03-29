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
using StructureMap.AutoMocking;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UserRequestCompleteActivityCommandHandlerSpecs
{
    public class when_completing_an_activity_and_entering_the_next_map_state_return_false : WithFakes
    {
        private static AutoMocker<UserRequestCompleteActivityCommandHandler> autoMocker;
        private static UserRequestCompleteActivityCommand userRequestCompleteActivityCommand;
        private static Activity activity;
        private static InstanceDataModel instance;
        private static WorkFlowDataModel workflow;
        private static HandleMapReturnCommand handleMapReturnCommand;
        private static Dictionary<string, string> mapVariables;
        private static IX2Process process;
        private static IX2ContextualDataProvider contextualData;
        private static IX2Map x2Map;
        private static int toStateId = 5;
        private static string userWhoPerformedTheActivity = "user";
        private static int? fromStateId = 1;
        private static string stageTransitionComments = "comments";
        private static string alertMessage = "toState";
        private static string activityMessage;
        private static long instanceID = 0;
        static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<UserRequestCompleteActivityCommandHandler>();
            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            contextualData = An<IX2ContextualDataProvider>();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
            activityMessage = "activityMessage";
            instance = new InstanceDataModel(instanceID, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            mapVariables = new Dictionary<string, string>();
            activity = new Activity(0, "activityName", fromStateId, "fromStataName", toStateId, "toState", 1, false);
            userRequestCompleteActivityCommand = new UserRequestCompleteActivityCommand(instanceID, activity, userWhoPerformedTheActivity, false, mapVariables);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceID)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(userRequestCompleteActivityCommand.InstanceId)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);
            x2Map.WhenToldTo(x => x.GetActivityMessage(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(activityMessage);
            x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(stageTransitionComments);
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            x2Map.WhenToldTo(x => x.CompleteActivity(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>(), ref activityMessage))
                .Return(true);
            x2Map.WhenToldTo(x => x.ExitState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(true);
            x2Map.WhenToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(false);
            handleMapReturnCommand = new HandleMapReturnCommand(instance.ID, false, Param.IsAny<ISystemMessageCollection>(), activity.ActivityName, WorkflowMapCodeSectionEnum.OnEnter);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>
                (x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(handleMapReturnCommand), metadata))
                .Callback<HandleMapReturnCommand>(
                y => { y.Result = false; }
                );
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>
                (x => x.CheckRuleCommand(Arg.Is<RuleCommand>(handleMapReturnCommand), metadata))
                .Callback<RuleCommand>(
                y => { y.Result = true; }
                );
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(userRequestCompleteActivityCommand, metadata);
        };

        private It should_not_save_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(Param.IsAny<SaveInstanceCommand>()), metadata));
        };

        private It should_not_record_the_workflow_history = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<SaveWorkflowHistoryCommand>(Param.IsAny<SaveWorkflowHistoryCommand>()), metadata));
        };

        private It should_not_process_system_requests = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                 x => x.HandleCommand(Arg.Is<BuildSystemRequestToProcessCommand>(Param.IsAny<BuildSystemRequestToProcessCommand>()), metadata));
        };

        private It should_not_refresh_the_worklist = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<RefreshWorklistCommand>(Param.IsAny<RefreshWorklistCommand>()), metadata));
        };

        private It should_not_refresh_the_security_for_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<RefreshInstanceActivitySecurityCommand>(Param.IsAny<RefreshInstanceActivitySecurityCommand>()), metadata));
        };

        private It should_not_record_the_stage_transition = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<RecordStageTransitionCommand>(Param.IsAny<RecordStageTransitionCommand>()), metadata));
        };
    };
}