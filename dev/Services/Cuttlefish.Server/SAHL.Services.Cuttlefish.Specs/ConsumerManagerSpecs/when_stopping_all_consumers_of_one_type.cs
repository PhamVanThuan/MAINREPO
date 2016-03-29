using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Specs.Fakes;

namespace SAHL.Services.Cuttlefish.Specs.ConsumerManagerSpecs
{
    public class when_stopping_all_consumers_of_one_type : WithFakes
    {
        private static IQueueConsumerManager workerManager;
        private static IQueueConsumerFactory queueConsumerFactory;
        private static string messageExchange;
        private static string messageQueue;
        private static int numberOfConsumersToStart;

        private Establish context = () =>
            {
                queueConsumerFactory = new FakeConsumerFactory();

                workerManager = new QueueConsumerManager(queueConsumerFactory);
                messageExchange = "testExchange";
                messageQueue = "testQueue";
                numberOfConsumersToStart = 2;
            };

        private Because of = () =>
            {
                workerManager.StartConsumer(messageExchange, messageQueue, numberOfConsumersToStart, new FakeConsumerWorker());

                workerManager.StopAllConsumers();
            };

        private It should_stop_all_consumers = () =>
        {
            workerManager.GetNumberOfRunningConsumers().ShouldEqual(0);
        };
    }
}