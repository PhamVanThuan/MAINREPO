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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandlerSpecs
{
    public class when_handling_a_system_request_and_the_activity_on_complete_code_returns_false : WithFakes
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
        private static long instanceId;
        private static IX2Map x2Map;
        private static IX2Process process;
        private static WorkFlowDataModel workflow;
        private static HandleMapReturnCommand handleMapReturnCommand;
        private static ISystemMessageCollection systemMessageCollection;
        private static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommandHandler>();

            x2Map = An<IX2Map>();
            process = An<IX2Process>();
            systemMessageCollection = An<ISystemMessageCollection>();
            autoMocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(systemMessageCollection);
            instance = new InstanceDataModel(instanceId, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            mapVariables = new Dictionary<string, string>();
            contextualData = An<IX2ContextualDataProvider>();
            activity = new Activity(0, "activityName", 1, "fromStataName", 2, "toState", 1, false);
            command = new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(instanceId, activity, userName);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceId)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(command.InstanceId)).Return(instance);
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(instance.ID)).Return(process);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(x2Map);

            workflow = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storageTable", "storageKey", 1, "", null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            x2Map.WhenToldTo(x => x.GetStageTransition(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return("comments");
            x2Map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualData);
            x2Map.WhenToldTo(x => x.StartActivity(instance, contextualData, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(true);
            x2Map.WhenToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(false);
            handleMapReturnCommand = new HandleMapReturnCommand(instance.ID, false, systemMessageCollection, activity.ActivityName, WorkflowMapCodeSectionEnum.OnComplete);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(
            x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(handleMapReturnCommand), metadata)).
            Callback<HandleMapReturnCommand>((c) => { c.Result = false; });
        };

        Because of = () =>
        {
            messages = autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_not_run_the_on_exit_code_for_the_state = () =>
        {
            x2Map.WasNotToldTo(x => x.ExitState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        It should_return_the_system_message_collection = () =>
        {
            messages.ShouldBeTheSameAs(systemMessageCollection);
        };
    }
}