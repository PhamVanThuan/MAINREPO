using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Core.Messaging.RabbitMQ;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.RabbitMQConsumerManagerSpecs
{
    public class when_initialising : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RabbitMQConsumerManager> autoMocker;

        Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RabbitMQConsumerManager>();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
        };

        It should_tell_the_consumer_configuration_provider_to_get_all_instances = () =>
        {
            autoMocker.Get<IIocContainer>().WasToldTo(x => x.GetAllInstances<IX2ConsumerConfigurationProvider>());
        };

        It should_tell_the_queue_consumer_manager_to_start_consumers = () =>
        {
            autoMocker.Get<IQueueConsumerManager>().WasToldTo(x => x.StartConsumers(Param.IsAny<List<QueueConsumerConfiguration>>()));
        };
    }
}