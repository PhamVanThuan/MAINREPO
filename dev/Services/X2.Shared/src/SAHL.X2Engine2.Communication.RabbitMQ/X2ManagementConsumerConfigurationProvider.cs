using Newtonsoft.Json;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.Serialisation;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.MessageHandlers;
using SAHL.X2Engine2.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class X2ManagementConsumerConfigurationProvider : IX2ManagementSubscriber
    {
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        private List<QueueConsumerConfiguration> queueConsumers;
        private IX2EngineConfigurationProvider configurationProvider;

        public X2ManagementConsumerConfigurationProvider(IX2EngineConfigurationProvider configurationProvider, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.configurationProvider = configurationProvider;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;

            this.queueConsumers = new List<QueueConsumerConfiguration>();
        }

        public void Subscribe(IX2RouteEndpoint route, IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest> messageHandler)
        {
            Action<string> action = (message) =>
            {
                try
                {
                    var deserializedMessage = JsonConvert.DeserializeObject<X2WrappedRequest>(message, JsonSerialisation.Settings);
                    messageHandler.HandleCommand((X2NotificationOfNewScheduledActivityRequest)deserializedMessage.X2Request);
                }
                catch (Exception ex)
                {
                    string exceptionMessage = ex.Message;
                    logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, ex);
                }
            };

            var numberOfManagementConsumers = this.configurationProvider.GetNumberOfManagementConsumers();
            queueConsumers.Add(new QueueConsumerConfiguration(route.ExchangeName, route.QueueName, numberOfManagementConsumers, action));
        }

        public List<QueueConsumerConfiguration> GetConsumers()
        {
            return this.queueConsumers;
        }
    }
}