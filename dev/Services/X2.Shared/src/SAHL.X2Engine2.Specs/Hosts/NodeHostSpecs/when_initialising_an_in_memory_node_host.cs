using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node;

namespace SAHL.X2Engine2.Specs.Hosts.NodeHostSpecs
{
    public class when_initialising_an_in_memory_node_host : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineNodeHost> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineNodeHost>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Start();
        };

        private It should_initialise_the_x2EngineNode = () =>
        {
            autoMocker.Get<IX2EngineNode>().WasToldTo(x => x.Initialise());
        };
    }
}