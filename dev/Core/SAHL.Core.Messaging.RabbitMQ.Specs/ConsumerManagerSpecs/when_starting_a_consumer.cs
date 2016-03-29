using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Messaging.RabbitMQ.Specs.Fakes;
using SAHL.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.ConsumerManagerSpecs
{
    public class when_starting_a_consumer : WithFakes
    {
        static IQueueConsumerManager queueConsumerManager;
        static QueueConsumerConfiguration config;
        static IQueueConnectionFactory connectionFactory;
        static IQueueConnection connection;
        static IQueueConsumerFactory consumerFactory;
        static IQueueConsumer consumer;
        static IRunnableTaskManager taskManager;
        static IRunnableTask task;
        static Action<string> workAction;

        Establish context = () =>
        {
            workAction = (messsage) => { };

            connection = An<IQueueConnection>();
            consumer = An<IQueueConsumer>();
            connectionFactory = An<IQueueConnectionFactory>();
            connectionFactory.WhenToldTo(x => x.CreateConnection())
                .Return(connection);

            consumerFactory = An<IQueueConsumerFactory>();
            consumerFactory.WhenToldTo(x=>x.CreateConsumer(Param.IsAny<IQueueConnection>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Action<string>>(), Param.IsAny<Func<bool>>(), Param.IsAny<string>()))
                .Return(consumer);

            task = An<IRunnableTask>();
            taskManager = An<IRunnableTaskManager>();
            taskManager.WhenToldTo(x => x.CreateTask(Param.IsAny<Action>(), Param.IsAny<CancellationToken>(), Param.IsAny<TaskCreationOptions>()))
                .Return(task);

            config = new QueueConsumerConfiguration("exchange", "queue", 1, workAction);

            queueConsumerManager = new QueueConsumerManager(connectionFactory, consumerFactory, taskManager);
        };

        Because of = () =>
        {
            queueConsumerManager.StartConsumers(new QueueConsumerConfiguration[] { config });
        };

        It should_connect_to_rabbitmq = () =>
        {
            connectionFactory.WasToldTo(x => x.CreateConnection());
            connection.WasToldTo(x => x.Connect());
        };

        It should_create_a_consumer = () =>
        {
            consumerFactory.WasToldTo(x => x.CreateConsumer(connection, "exchange", "queue", workAction, Arg.Any <Func<bool>>(), Arg.Any<string>()));
        };

        It should_start_the_consumer = () =>
        {
            task.WasToldTo(x => x.Start());
        };

        Cleanup afterwards = () =>
        {
            queueConsumerManager.StopAllConsumers();
        };

    }
}
