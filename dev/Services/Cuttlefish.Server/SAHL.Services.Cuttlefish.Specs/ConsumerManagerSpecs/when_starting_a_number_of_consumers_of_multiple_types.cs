using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Specs.Fakes;

namespace SAHL.Services.Cuttlefish.Specs.ConsumerManagerSpecs
{
    public class when_starting_a_number_of_consumers_of_multiple_types : WithFakes
    {
        private static IQueueConsumerManager workerManager;
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

                workerManager = new QueueConsumerManager(queueConsumerFactory);
                messageExchange = "testExchange";
                messageQueue = "testQueue";
                numberOfConsumersToStart = 2;
                messageExchange2 = "testExchange";
                messageQueue2 = "testQueue";
                numberOfConsumersToStart2 = 3;
            };

        private Because of = () =>
            {
                workerManager.StartConsumer(messageExchange, messageQueue, numberOfConsumersToStart, new FakeConsumerWorker());
                workerManager.StartConsumer(messageExchange2, messageQueue2, numberOfConsumersToStart2, new FakeConsumerWorker());
            };

        private It should_start_the_process_the_requested_number_of_times = () =>
            {
                workerManager.GetNumberOfRunningConsumers().ShouldEqual(numberOfConsumersToStart + numberOfConsumersToStart2);
            };

        private Cleanup stopProcesses = () =>
            {
                workerManager.StopAllConsumers();
            };
    }
}