using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Hosts;

namespace SAHL.X2Engine2.Specs.Hosts.InMemoryHostSpecs
{
    public class when_initialising_an_in_memory_host : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineHost> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineHost>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Start();
        };

        private It should_initialise_the_x2engine = () =>
        {
            autoMocker.Get<IX2Engine>().WasToldTo(x => x.Initialise());
        };
    }
}