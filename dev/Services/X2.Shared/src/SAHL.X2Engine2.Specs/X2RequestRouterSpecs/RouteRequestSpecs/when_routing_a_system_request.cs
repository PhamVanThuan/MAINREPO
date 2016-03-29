
using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2RequestRouterSpecs.RouteRequestSpecs
{
    public class when_routing_a_system_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestRouter> autoMocker;

        private static X2Request request;
        private static IX2RouteEndpoint route;
        private static X2Workflow workflow;
        private static IX2RequestMonitor requestMonitor;
        private static bool isUserRequest = false;
        private static IServiceRequestMetadata metadata;

        Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestRouter>();
            requestMonitor = An<IX2RequestMonitor>();
            request = new X2RequestForExistingInstance(Guid.NewGuid(), 999, X2RequestType.UserCreate, metadata, "activityName", false);
            workflow = new X2Workflow("processName", "workflowName");
            route = new X2RouteEndpoint("e", "q");
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(request)).Return(workflow);
            autoMocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.IsRequestMonitored(request)).Return(isUserRequest);
            autoMocker.Get<IX2RoutePlanner>().WhenToldTo(x => x.PlanRoute(isUserRequest, workflow)).Return(route);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.RouteRequest(request, requestMonitor);
        };

        It should_get_the_workflow_for_the_request = () =>
        {
            autoMocker.Get<IX2RequestInterrogator>().WasToldTo(x => x.GetRequestWorkflow(request));
        };

        It should_not_monitor_the_request = () =>
        {
            requestMonitor.WasNotToldTo(x => x.MonitorRequest(request));
        };

        It should_get_a_planned_route = () =>
        {
            autoMocker.Get<IX2RoutePlanner>().WasToldTo(x => x.PlanRoute(isUserRequest, workflow));
        };

        It should_route_the_request = () =>
        {
            autoMocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish(route, request));
        };
    }
}