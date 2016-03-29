using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Messaging.RabbitMQ;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.RabbitMQConsumerManagerSpecs
{
    public class when_told_to_tear_down : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RabbitMQConsumerManager> autoMocker;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RabbitMQConsumerManager>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.TearDown();
        };

        It should_tell_the_queue_consumer_manager_to_start_consumers = () =>
        {
            autoMocker.Get<IQueueConsumerManager>().WasToldTo(x => x.StopAllConsumers());
        };
    }
}