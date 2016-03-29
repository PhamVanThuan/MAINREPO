using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UserRequestCompleteCreateCommandHandlerSpecs
{
    public class when_completing_the_create_of_an_instance_and_entering_the_initial_state_returns_false : WithFakes
    {
        private static AutoMocker<UserRequestCompleteCreateCommandHandler> autoMocker = new NSubstituteAutoMocker<UserRequestCompleteCreateCommandHandler>();
        private static UserRequestCompleteCreateCommand command;
        private static long instanceID = 0;
        private static Activity activity;
        private static InstanceDataModel instance;
        private static IX2ContextualDataProvider contextualData;
        private static IX2Map x2Map;
        private static Dictionary<string, string> mapVariables;
        private static int toStateId = 1;
        private static string userWhoPerformedTheActivity = "user";
        private static int? fromStateId = null;
        private static string stageTransitionComments = "comments";
        private static IX2Process process;
        private static string alertMessage = "toState";
        private static WorkFlowDataModel workflow;
        private static HandleMapReturnCommand mapReturnCommand;
        private static string activityName;
        private static ISystemMessageCollection messages;
        private static ISystemMessageCollection returnedMessages;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            messages = An<ISystemMessageCollection>();
            instance = new InstanceDataModel(instanceID, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            mapVariables = new Dictionary<string, string>();
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(0, "create", fromStateId, "", toStateId, "toState", 1, false);
            activityName = activity.ToStateName;
            command = new UserRequestCompleteCreateCommand(instanceID, activity, userWhoPerformedTheActivity, false, mapVariables);

            autoMocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(messages);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceID))
                .Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(command.InstanceId))
                .Return(instance);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID))
                .Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>()))
                .Return(x2Map);
            x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(stageTransitionComments);
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>()))
                .Return(contextualData);
            x2Map.WhenToldTo(x => x.CompleteActivity(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(),
                Param.IsAny<ISystemMessageCollection>(), ref activityName)).Return(true);
            x2Map.WhenToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(),
                Param.IsAny<ISystemMessageCollection>())).Return(false);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>()))
                .Return(workflow);
            mapReturnCommand = new HandleMapReturnCommand(instance.ID, false, Param.IsAny<ISystemMessageCollection>(), activity.ActivityName, WorkflowMapCodeSectionEnum.OnEnter);
            autoMocker.Get<IX2ServiceCommandRouter>()
                .WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(mapReturnCommand), metadata))
                .Callback<HandleMapReturnCommand>(y => { y.Result = false; });
        };

        private Because of = () =>
        {
            returnedMessages = autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_return_the_message_collection = () =>
            {
                returnedMessages.ShouldBeTheSameAs(messages);
            };

        private It should_not_save_the_workflow_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(Param.IsAny<SaveContextualDataCommand>()), metadata));
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
    }
}