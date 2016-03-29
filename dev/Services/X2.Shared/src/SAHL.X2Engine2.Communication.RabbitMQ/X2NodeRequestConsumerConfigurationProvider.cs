using Newtonsoft.Json;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.Serialisation;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Node.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class X2NodeRequestConsumerConfigurationProvider : IX2RequestSubscriber
    {
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        private IX2NodeConfigurationProvider x2NodeConfigurationProvider;
        private IX2QueueNameBuilder x2QueueNameBuilder;
        private IQueueConsumerManager queueConsumerManager;
        private IMessageProcessor<IX2Request> requestProcessor;
        private List<QueueConsumerConfiguration> queueConsumers;

        public X2NodeRequestConsumerConfigurationProvider(IX2NodeConfigurationProvider x2NodeConfigurationProvider, IX2QueueNameBuilder x2QueueNameBuilder, IQueueConsumerManager queueConsumerManager, IMessageProcessor<IX2Request> requestProcessor, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;

            this.x2NodeConfigurationProvider = x2NodeConfigurationProvider;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
            this.queueConsumerManager = queueConsumerManager;
            this.requestProcessor = requestProcessor;
            this.queueConsumers = new List<QueueConsumerConfiguration>();
        }

        public void Initialise()
        {
            Action<string> action = (message) =>
            {
                try
                {
                    var deserializedMessage = JsonConvert.DeserializeObject<X2WrappedRequest>(message, JsonSerialisation.Settings);
                    this.requestProcessor.ProcessMessage(deserializedMessage.X2Request);
                }
                catch (Exception ex)
                {
                    string exceptionMessage = ex.Message;
                    logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, ex);
                }
            };

            IEnumerable<X2WorkflowConfiguration> workflowConfigurations = this.x2NodeConfigurationProvider.GetWorkflowConfigurations();
            foreach (X2WorkflowConfiguration workflowConfiguration in workflowConfigurations)
            {
                X2Workflow workflow = new X2Workflow(workflowConfiguration.ProcessName, workflowConfiguration.WorkflowName);
                var userQueue = this.x2QueueNameBuilder.GetUserQueue(workflow);
                queueConsumers.Add(new QueueConsumerConfiguration(userQueue.ExchangeName, userQueue.QueueName, workflowConfiguration.UserConsumers, action));

                var systemQueue = this.x2QueueNameBuilder.GetSystemQueue(workflow);
                queueConsumers.Add(new QueueConsumerConfiguration(systemQueue.ExchangeName, systemQueue.QueueName, workflowConfiguration.SystemConsumers, action));
            }
        }

        public List<QueueConsumerConfiguration> GetConsumers()
        {
            return queueConsumers;
        }
    }
}