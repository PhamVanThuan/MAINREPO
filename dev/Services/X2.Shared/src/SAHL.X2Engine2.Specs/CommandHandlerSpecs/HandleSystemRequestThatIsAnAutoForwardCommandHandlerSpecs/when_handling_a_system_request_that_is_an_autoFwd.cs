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
    public class when_handling_a_system_request_that_is_an_autoFwd : WithFakes
    {
        private static AutoMocker<HandleSystemRequestThatIsAnAutoForwardCommandHandler> automocker = new NSubstituteAutoMocker<HandleSystemRequestThatIsAnAutoForwardCommandHandler>();
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
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            messages = new SystemMessageCollection();
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
            automocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(messages);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(instance);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById(Param.IsAny<int>())).Return(state);
            automocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<DeleteAllScheduleActivitiesCommand>(c => c.InstanceId == command.InstanceId), metadata)
                    .Returns(new SystemMessageCollection()));
            automocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(Param.IsAny<long>())).Return(process);
            map.WhenToldTo(x => x.GetForwardStateName(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()))
                .Return(autoFwdStateName);
            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);
            map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualDataProvider);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateDataModel(Param.IsAny<string>(), Param.IsAny<string>())).Return(forwardState);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(Param.IsAny<InstanceDataModel>())).Return(workflow.Name);
            map.WhenToldTo(x => x.EnterState(Param.IsAny<InstanceDataModel>(), Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(true);
            automocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(c => c.InstanceId == command.InstanceId), metadata)
                    .Returns(new SystemMessageCollection()));
            automocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(c => c.Instance.ID == instance.ID), metadata)
                    .Returns(new SystemMessageCollection()));
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_create_an_empty_message_collection = () =>
        {
            automocker.Get<IMessageCollectionFactory>().WasToldTo(x => x.CreateEmptyCollection());
        };

        It should_get_the_instance_data_from_workflow_data_provider = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(command.InstanceId));
        };

        It should_get_the_workflow_for_instance_from_the_workflow_data_provider = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflowById(instance.WorkFlowID));
        };

        It should_get_the_current_state_of_the_instance_from_the_workflow_data_provider = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateById((int)instance.StateID));
        };

        It should_remove_any_existing_scheduled_activities = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<DeleteAllScheduleActivitiesCommand>(c => c.InstanceId == command.InstanceId), metadata));
        };

        It should_get_the_x2process_for_the_instance = () =>
        {
            automocker.Get<IX2ProcessProvider>().WasToldTo(x => x.GetProcessForInstance(instanceId));
        };

        It should_get_the_x2map = () =>
        {
            process.WasToldTo(x => x.GetWorkflowMap(Arg.Is<string>(i => i.ToString().Equals(workflow.Name))));
        };

        It should_get_the_contextual_data_provider = () =>
        {
            map.WasToldTo(x => x.GetContextualData(instance.ID));
        };

        It should_get_the_forward_state_name_from_the_x2_map = () =>
        {
            map.WasToldTo(x => x.GetForwardStateName(instance, contextualDataProvider, Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
        };

        It should_get_the_forward_state_details = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateDataModel(autoFwdStateName, workflow.Name));
        };

        It should_run_the_on_enter_code_in_the_x2_map_for_the_state_being_forwarded_to = () =>
        {
            map.WasToldTo(x => x.EnterState(instance, contextualDataProvider, Arg.Is<IX2Params>(y => y.StateName == autoFwdStateName), messages));
        };

        It should_save_the_contextual_data = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(c => c.InstanceId == command.InstanceId), metadata));
        };

        It should_save_the_instance = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(c => c.Instance.ID == instance.ID), metadata));
        };

        It should_build_up_a_list_system_requests_to_process = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<BuildSystemRequestToProcessCommand>(c => c.Instance.ID == instance.ID), metadata));
        };

        It should_refresh_the_worklist = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshWorklistCommand>(c => c.Instance.ID == instance.ID), metadata));
        };

        It should_refresh_the_instance_activity_security = () =>
        {
            automocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<RefreshInstanceActivitySecurityCommand>(c => c.Instance.ID == instance.ID), metadata));
        };
    }
}