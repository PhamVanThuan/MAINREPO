using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Messaging.RabbitMQ.Specs.Fakes;
using SAHL.Core.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerManagerSpecs
{
    public class when_disposing : WithFakes
    {
        private static IQueueConsumerManager queueConsumerManager;
        private static QueueConsumerConfiguration config;
        private static IQueueConnectionFactory connectionFactory;
        private static IQueueConnection connection;
        private static IQueueConsumerFactory consumerFactory;
        private static IQueueConsumer consumer;
        private static IRunnableTaskManager taskManager;
        private static IRunnableTask task;
        private static Action<string> workAction;
        private static FakeRunnableTaskCancellation cancellation;

        private Establish context = () =>
        {
            workAction = (messsage) => { };

            connection = An<IQueueConnection>();
            consumer = An<IQueueConsumer>();
            connectionFactory = An<IQueueConnectionFactory>();
            connectionFactory.WhenToldTo(x => x.CreateConnection())
                .Return(connection);

            consumerFactory = An<IQueueConsumerFactory>();
            consumerFactory.WhenToldTo(x => x.CreateConsumer(Param.IsAny<IQueueConnection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Action<string>>(), Param.IsAny<Func<bool>>(), Param.IsAny<string>()))
                .Return(consumer);

            cancellation = new FakeRunnableTaskCancellation();
            task = An<IRunnableTask>();
            taskManager = An<IRunnableTaskManager>();
            taskManager.WhenToldTo(x => x.CreateTask(Param.IsAny<Action>(), Param.IsAny<CancellationToken>(), Param.IsAny<TaskCreationOptions>()))
                .Return(task);

            taskManager.WhenToldTo(x => x.BuildTaskCancellation())
               .Return(cancellation);

            config = new QueueConsumerConfiguration("exchange", "queue", 1, workAction);

            queueConsumerManager = new QueueConsumerManager(connectionFactory, consumerFactory, taskManager);
        };

        private Because of = () =>
        {
            queueConsumerManager.StartConsumers(new QueueConsumerConfiguration[] { config });
            queueConsumerManager.Dispose();
        };

        private It should_stop_the_consumers = () =>
        {
            cancellation.WasCancelled.ShouldBeTrue();
        };

        It should_dispose_the_rabbitmq_connection = () =>
        {
            connection.WasToldTo(x => x.Dispose());
        };
    }
}