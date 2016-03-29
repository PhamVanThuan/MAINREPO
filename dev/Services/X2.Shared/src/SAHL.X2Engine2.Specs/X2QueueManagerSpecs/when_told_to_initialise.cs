using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Specs.X2QueueManagerSpecs
{
    public class when_told_to_initialise : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2QueueManager> autoMocker;
        private static List<X2Workflow> supportedWorkflows = null;
        private static X2RouteEndpoint route;
        private static X2Workflow workflow = new X2Workflow("process", "workflow");
        private static Dictionary<X2Workflow, List<IX2RouteEndpoint>> declaredWorkflowRoutes;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2QueueManager>();
            supportedWorkflows = new List<X2Workflow> { workflow };
            route = new X2RouteEndpoint(workflow.ProcessName, workflow.WorkflowName);
            autoMocker.Get<IX2EngineConfigurationProvider>().WhenToldTo(x => x.GetSupportedWorkflows()).Return(supportedWorkflows);
            autoMocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetExchanges(Arg.Is<X2Workflow>(y => y.ProcessName == "process" && y.WorkflowName == "workflow"))).Return(new List<string>() { route.ExchangeName });
            autoMocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetQueues(Arg.Is<X2Workflow>(y => y.ProcessName == "process" && y.WorkflowName == "workflow"))).Return(new List<IX2RouteEndpoint>() { route });
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
            declaredWorkflowRoutes = autoMocker.ClassUnderTest.DeclaredWorkflowRoutes;
        };

        private It should_get_supported_workflows = () =>
        {
            autoMocker.Get<IX2EngineConfigurationProvider>().WasToldTo(x => x.GetSupportedWorkflows());
        };

        private It should_tell_message_bus_to_declare_workflow_exchange = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareExchange(Arg.Is<string>(y => y == route.ExchangeName)));
        };

        private It should_tell_message_bus_to_declare_workflow_queue = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareQueue(Arg.Is<string>(y => y == route.ExchangeName), Arg.Is<string>(y => y == route.QueueName),true));
        };

        private It should_update_declared_workflow_routes = () =>
        {
            declaredWorkflowRoutes.ShouldNotBeNull();
            declaredWorkflowRoutes.First().Key.ShouldEqual(workflow);
            declaredWorkflowRoutes.First().Value.First().ShouldEqual(route);
        };

        private It should_tell_message_bus_to_declare_error_exchange = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareExchange(Arg.Is<string>(y => y == X2QueueManager.X2EngineErrorExchange)));
        };

        private It should_tell_message_bus_to_declare_error_queue = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareQueue(Arg.Is<string>(y => y == X2QueueManager.X2EngineErrorExchange), Arg.Is<string>(y => y == X2QueueManager.X2EngineErrorQueue), true));
        };

        private It should_tell_message_bus_to_declare_timer_refresh_exchange = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareExchange(Arg.Is<string>(y => y == X2QueueManager.X2EngineTimersRefreshExchange)));
        };

        private It should_tell_message_bus_to_declare_timer_refresh_queue = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareQueue(Arg.Is<string>(y => y == X2QueueManager.X2EngineTimersRefreshExchange), Arg.Is<string>(y => y == X2QueueManager.X2EngineTimersRefreshQueue), true));
        };

        private It should_tell_message_bus_to_declare_response_exchange = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareExchange(Arg.Is<string>(y => y == X2QueueManager.X2EngineResponseExchange)));
        };

        private It should_tell_message_bus_to_declare_should_tell_message_bus_to_declare_respons_queue = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.DeclareQueue(Arg.Is<string>(y => y == X2QueueManager.X2EngineResponseExchange), Arg.Is<string>(y => y == X2QueueManager.X2EngineResponseQueue), true));
        };
    }
}