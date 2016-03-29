using Machine.Fakes;
using Machine.Specifications;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2RequestRouterSpecs.RouteRequestSpecs
{
    public class when_initialising_a_request_router : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestRouter> autoMocker;

        Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<X2RequestRouter>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        It should_initialise_the_route_planner = () =>
        {
            autoMocker.Get<IX2RoutePlanner>().WasToldTo(x => x.Initialise());
        };
    }
}