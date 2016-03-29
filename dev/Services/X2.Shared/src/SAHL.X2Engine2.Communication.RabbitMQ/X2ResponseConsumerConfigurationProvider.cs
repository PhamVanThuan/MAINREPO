using Newtonsoft.Json;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.Serialisation;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class X2ResponseConsumerConfigurationProvider : IX2ResponseSubscriber
    {
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        private List<QueueConsumerConfiguration> queueConsumers;
        private IX2EngineConfigurationProvider configurationProvider;

        public X2ResponseConsumerConfigurationProvider(IX2EngineConfigurationProvider configurationProvider, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;

            this.configurationProvider = configurationProvider;
            this.queueConsumers = new List<QueueConsumerConfiguration>();
        }

        public void Subscribe<TResponse>(Action<TResponse> responseCallback) where TResponse : Core.X2.Messages.X2Response
        {
            Action<string> action = (message) =>
            {
                try
                {
                    var deserializedMessage = JsonConvert.DeserializeObject<X2Response>(message, JsonSerialisation.Settings);
                    responseCallback((TResponse)deserializedMessage);
                }
                catch (Exception ex)
                {
                    string exceptionMessage = ex.Message;
                    logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, ex);
                }
            };

            var numberOfResponseConsumers = this.configurationProvider.GetNumberOfResponseConsumers();
            queueConsumers.Add(new QueueConsumerConfiguration(X2QueueManager.X2EngineResponseExchange, X2QueueManager.X2EngineResponseQueue, numberOfResponseConsumers, action));
        }

        public List<QueueConsumerConfiguration> GetConsumers()
        {
            return this.queueConsumers;
        }
    }
}