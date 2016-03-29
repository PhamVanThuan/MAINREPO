using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Specs.Fakes;

namespace SAHL.Services.Cuttlefish.Specs.ConsumerManagerSpecs
{
    public class when_stopping_all_consumers_of_multiple_types : WithFakes
    {
        private static IQueueConsumerManager consumerManager;
        private static IQueueConsumerFactory queueConsumerFactory;
        private static string messageExchange;
        private static string messageQueue;
        private static string messageExchange2;
        private static string messageQueue2;
        private static int numberOfConsumersToStart;
        private static int numberOfConsumersToStart2;

        private Establish context = () =>
            {
                queueConsumerFactory = new FakeConsumerFactory();

                consumerManager = new QueueConsumerManager(queueConsumerFactory);
                messageExchange = "testExchange";
                messageQueue = "testQueue";
                numberOfConsumersToStart = 2;
                messageExchange2 = "testExchange";
                messageQueue2 = "testQueue";
                numberOfConsumersToStart2 = 3;
            };

        private Because of = () =>
            {
                consumerManager.StartConsumer(messageExchange, messageQueue, numberOfConsumersToStart, new FakeConsumerWorker());
                consumerManager.StartConsumer(messageExchange2, messageQueue2, numberOfConsumersToStart2, new FakeConsumerWorker());

                consumerManager.StopAllConsumers();
            };

        private It should_stop_all_consumers = () =>
        {
            consumerManager.GetNumberOfRunningConsumers().ShouldEqual(0);
        };
    }
}