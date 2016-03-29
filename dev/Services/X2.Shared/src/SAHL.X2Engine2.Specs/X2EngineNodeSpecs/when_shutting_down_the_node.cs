using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node;
using System;

namespace SAHL.X2Engine2.Specs.X2EngineNodeSpecs
{
    public class when_shutting_down_the_node : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2EngineNode> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2EngineNode>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Teardown();
        };

        private It should_call_teardown_on_the_consumer_manager = () =>
        {
            autoMocker.Get<IX2ConsumerManager>().WasToldTo(x => x.TearDown());
        };
    }
}