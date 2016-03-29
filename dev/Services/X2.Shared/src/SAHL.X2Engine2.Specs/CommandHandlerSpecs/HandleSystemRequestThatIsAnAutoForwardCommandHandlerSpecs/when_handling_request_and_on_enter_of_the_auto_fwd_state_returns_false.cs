using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.HandleSystemRequestThatIsAnAutoForwardCommandHandlerSpecs
{
    public class when_handling_request_and_on_enter_of_the_auto_fwd_state_returns_false : WithFakes
    {
        private static AutoMocker<HandleSystemRequestThatIsAnAutoForwardCommandHandler> autoMocker = new NSubstituteAutoMocker<HandleSystemRequestThatIsAnAutoForwardCommandHandler>();
        private static HandleSystemRequestThatIsAnAutoForwardCommand command;
        private static long instanceId;
        private static string userName;
        private static InstanceDataModel instance;
        private static WorkFlowDataModel workflow;
        private static StateDataModel state;
        private static ISystemMessageCollection messages;
        private static IX2Process process;
        private static string autoFwdStateName;
        private static StateDataModel forwardState;
        private static IX2Map map;
        private static IX2ContextualDataProvider contextualDataProvider;
        private static HandleMapReturnCommand handleMapReturnCommand;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            instanceId = 123465789L;
            userName = @"SAHL\ClintonS";
            command = new HandleSystemRequestThatIsAnAutoForwardCommand(instanceId, userName);
            instance = Helper.GetInstanceDataModel(instanceId);
            workflow = Helper.GetWorkflowDataModel();
            state = Helper.GetStateDataModel();
            IEnumerable<X2Workflow> workflows = new[] { new X2Workflow("Process", "Workflow") };
            autoFwdStateName = "AutoForwardState";
            forwardState = Helper.GetStateDataModel();
            forwardState.Name = autoFwdStateName;
            process = An<IX2Process>();
            map = An<IX2Map>();
            contextualDataProvider = An<IX2ContextualDataProvider>();
            autoMocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(messages);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById(Param.IsAny<int>())).Return(state);
            autoMocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<DeleteAllScheduleActivitiesCommand>(c => c.InstanceId == command.InstanceId), metadata)
                    .Returns(new SystemMessageCollection()));
            autoMocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(Param.IsAny<long>())).Return(process);
            map.WhenToldTo(x => x.GetForwardStateName(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(autoFwdStateName);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);
            map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualDataProvider);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateDataModel(Param.IsAny<string>(), Param.IsAny<string>())).Return(forwardState);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(Param.IsAny<InstanceDataModel>())).Return(workflow.Name);
            map.WhenToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(false);
            handleMapReturnCommand = new HandleMapReturnCommand(instance.ID, false, messages, string.Empty, WorkflowMapCodeSectionEnum.OnEnter);
            autoMocker.Get<IX2ServiceCommandRouter>().WhenToldTo<IX2ServiceCommandRouter>(
            x => x.HandleCommand(Arg.Is<HandleMapReturnCommand>(handleMapReturnCommand), metadata)).Callback<HandleMapReturnCommand>((c) => { c.Result = false; });
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_not_save_the_contextual_data = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(Arg.Any<SaveContextualDataCommand>()), metadata));
        };

        It should_not_save_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(Arg.Any<SaveInstanceCommand>()), metadata));
        };

        It should_not_queue_up_any_system_requests = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(x =>
                x.HandleCommand(Arg.Is<BuildSystemRequestToProcessCommand>(Arg.Any<BuildSystemRequestToProcessCommand>()), metadata));
        };

        It should_not_refresh_the_Worklist = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<RefreshWorklistCommand>(
                Arg.Any<RefreshWorklistCommand>()), metadata));
        };

        It should_not_refresh_the_security_for_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasNotToldTo<IX2ServiceCommandRouter>(x => x.HandleCommand(Arg.Is<RefreshInstanceActivitySecurityCommand>(
                Arg.Any<RefreshInstanceActivitySecurityCommand>()), metadata));
        };
    }
}