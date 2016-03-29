using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.X2EngineSpecs
{
    public class when_initialise : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2Engine> autoMocker;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2Engine>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        It should_initialise_the_request_router = () =>
        {
            autoMocker.Get<IX2RequestRouter>().WasToldTo(x => x.Initialise());
        };

        It should_initialise_the_request_monitor = () =>
        {
            autoMocker.Get<IX2RequestMonitorFactory>().WasToldTo(x => x.Initialise());
        };

        It should_initialise_the_activity_scheduler = () =>
        {
            autoMocker.Get<IX2ActivityScheduler>().WasToldTo(x => x.Initialise(autoMocker.ClassUnderTest));
        };

        It should_initialise_the_consumer_manager = () =>
        {
            autoMocker.Get<IX2ConsumerManager>().WasToldTo(x => x.Initialise());
        };

    }
}