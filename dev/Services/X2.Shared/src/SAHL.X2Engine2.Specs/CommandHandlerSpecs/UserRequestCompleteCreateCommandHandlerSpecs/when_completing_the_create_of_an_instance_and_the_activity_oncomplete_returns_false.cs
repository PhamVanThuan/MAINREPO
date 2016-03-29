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
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UserRequestCompleteCreateCommandHandlerSpecs
{
    public class when_completing_the_create_of_an_instance_and_the_activity_oncomplete_returns_false : WithFakes
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
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            instance = new InstanceDataModel(instanceID, 1, null, "name", "subject", "workflowProvider", fromStateId, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            mapVariables = new Dictionary<string, string>();
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(0, "create", fromStateId, "", toStateId, "toState", 1, false);
            activityName = activity.ToStateName;
            command = new UserRequestCompleteCreateCommand(instanceID, activity, userWhoPerformedTheActivity, false, mapVariables);
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
                Param.IsAny<ISystemMessageCollection>(), ref activityName)).Return(false);
            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>()))
                .Return(workflow);
            mapReturnCommand = new HandleMapReturnCommand(instance.ID, false, Param.IsAny<ISystemMessageCollection>(), activity.ToStateName, WorkflowMapCodeSectionEnum.OnComplete);
            autoMocker.Get<IX2ServiceCommandRouter>()
                .WhenToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(mapReturnCommand), metadata))
                .Callback<HandleMapReturnCommand>(y => { y.Result = false; });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_not_queue_up_external_activities = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(
                x => x.HandleCommand(Arg.Is<QueueUpExternalActivitiesCommand>(Param.IsAny<QueueUpExternalActivitiesCommand>()), metadata));
        };

        private It should_not_enter_the_next_state_in_the_map = () =>
        {
            x2Map.WasNotToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };
    }
}