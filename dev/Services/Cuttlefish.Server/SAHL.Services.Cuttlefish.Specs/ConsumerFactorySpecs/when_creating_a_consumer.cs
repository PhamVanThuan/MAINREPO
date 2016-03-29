using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Services.Cuttlefish.Managers;
using SAHL.Services.Cuttlefish.Specs.Fakes;
using System;

namespace SAHL.Services.Cuttlefish.Specs.ConsumerFactorySpecs
{
    public class when_creating_a_consumer : WithFakes
    {
        private static IQueueConsumerFactory consumerFactory;
        private static IQueueConsumer queueConsumer;
        private static IQueueConnectionFactory queueInstanceFactory;
        private static FakeMessageBusConfigurationProvider messageBusConfiguration;
        private static string exchangeName;
        private static string queueName;
        private static Func<bool> cancellationFunction;
        private static Action<string> workAction;
        private static ILogger logger;
        private static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();

            messageBusConfiguration = new FakeMessageBusConfigurationProvider();
            queueInstanceFactory = new QueueInstanceFactory(logger, loggerSource);
            consumerFactory = new QueueConsumerFactory(messageBusConfiguration, queueInstanceFactory, logger, loggerSource);
            exchangeName = "SomeExchange";
            queueName = "SomeQueue";
            cancellationFunction = () => { return false; };

            workAction = (message) => { };
        };

        private Because of = () =>
        {
            queueConsumer = consumerFactory.CreateQueueConsumer(exchangeName, queueName, workAction, cancellationFunction);
        };

        private It should_return_a_non_null_queue_consumer = () =>
        {
            queueConsumer.ShouldNotBeNull();
        };

        private It should_have_correctly_set_the_queue_servername_on_the_consumer = () =>
        {
            queueConsumer.QueueServerName.ShouldEqual(messageBusConfiguration.HostName);
        };

        private It should_have_correctly_set_the_queue_server_username_on_the_consumer = () =>
        {
            queueConsumer.Username.ShouldEqual(messageBusConfiguration.UserName);
        };

        private It should_have_correctly_set_the_queue_server_password_on_the_consumer = () =>
        {
            queueConsumer.Password.ShouldEqual(messageBusConfiguration.Password);
        };

        private It should_have_correctly_set_the_queue_server_exchange_on_the_consumer = () =>
        {
            queueConsumer.ExchangeName.ShouldEqual(exchangeName);
        };

        private It should_have_correctly_set_the_queue_server_queue_on_the_consumer = () =>
        {
            queueConsumer.QueueName.ShouldEqual(queueName);
        };

        private It should_have_correctly_set_the_cancellation_function_on_the_consumer = () =>
        {
            queueConsumer.ShouldCancel.ShouldEqual(cancellationFunction);
        };

        private It should_have_correctly_set_the_work_action_to_perform_on_the_consumer = () =>
        {
            queueConsumer.WorkAction.ShouldEqual(workAction);
        };
    }
}