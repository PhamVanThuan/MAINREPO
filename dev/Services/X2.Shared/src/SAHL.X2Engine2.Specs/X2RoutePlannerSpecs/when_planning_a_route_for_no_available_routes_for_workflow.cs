using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Specs.X2RoutePlannerSpecs
{
    public class when_planning_a_route_for_no_available_routes_for_workflow : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RoutePlanner> autoMocker;
        static IX2RouteEndpoint route;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RoutePlanner>();
        };

        private Because of = () =>
        {
            route = autoMocker.ClassUnderTest.PlanRoute(true, new X2Workflow("processName", "workflowName"));
        };

        private It should_return_null = () =>
        {
            route.ShouldBeNull();
        };
    }
}