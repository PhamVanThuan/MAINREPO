using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.X2EngineNodeSpecs
{
    public class when_initialising_an_x2engine_node : WithFakes
    {
        private static AutoMocker<X2EngineNode> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<X2EngineNode>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        private It should_initialise_the_node_management_message_subscriber = () =>
        {
            autoMocker.Get<IX2NodeManagementSubscriber>().WasToldTo(x => x.Initialise());
        };

        private It should_initialise_the_request_subscriber = () =>
        {
            autoMocker.Get<IX2RequestSubscriber>().WasToldTo(x => x.Initialise());
        };

        private It should_initialise_the_consumer_manager = () =>
        {
            autoMocker.Get<IX2ConsumerManager>().WasToldTo(x => x.Initialise());
        };

        private It should_initialise_the_process = () =>
        {
            autoMocker.Get<IX2ProcessProvider>().WasToldTo(x => x.Initialise());
        };
    }
}