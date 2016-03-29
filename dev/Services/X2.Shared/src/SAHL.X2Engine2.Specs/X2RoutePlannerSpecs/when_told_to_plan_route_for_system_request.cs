using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2RoutePlannerSpecs
{
    public class when_told_to_plan_route_for_system_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RoutePlanner> autoMocker;
        private static IX2RouteEndpoint route;
        private static bool isUserRequest = false;
        private static X2Workflow workflow = new X2Workflow("processName", "workflowName");
        private static string exchange = string.Format("{0}.{1}", workflow.ProcessName, workflow.WorkflowName);

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RoutePlanner>();
            autoMocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetSystemExchange(workflow)).Return(exchange);
        };

        private Because of = () =>
        {
            route = autoMocker.ClassUnderTest.PlanRoute(isUserRequest, workflow);
        };

        private It should_return_a_route = () =>
        {
            route.ShouldNotBeNull();
        };

        private It should_build_a_user_exchange = () =>
        {
            autoMocker.Get<IX2QueueNameBuilder>().WasToldTo(x => x.GetSystemExchange(workflow));
        };

        private It should_not_ask_the_consumer_monitor_if_the_queue_has_a_consumer = () =>
        {
            autoMocker.Get<IX2ConsumerMonitor>().WasNotToldTo(x => x.IsExchangeActive(exchange, workflow));
        };
    }
}