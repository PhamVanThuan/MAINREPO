using SAHL.Common.Logging;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public class MessageRetryService<TMessage> : IMessageRetryService<TMessage> where TMessage : class, IBatchMessage
    {
        private ITimer timer;
        private IRepository repository;
        private IBatchServiceConfiguration serviceConfiguration;
        private readonly IDiposableMessageBus messageBus;
        private ILogger logger;

        public MessageRetryService(ITimer timer, IRepository repository, IBatchServiceConfiguration serviceConfiguration, IDiposableMessageBus messageBus, ILogger logger)
        {
            this.timer = timer;
            this.repository = repository;
            this.serviceConfiguration = serviceConfiguration;
            this.messageBus = messageBus;
            this.logger = logger;
        }

        public void Start()
        {
            this.timer.Start(this.serviceConfiguration.TimeOutIntervalToReloadFailedMessages, RetryFailedMessages);
        }

        public void Reset()
        {
            this.timer.Reset();
        }

        public void RetryFailedMessages()
        {
            var failedMessages = this.repository.Load<TMessage>(GenericStatuses.Failed,this.serviceConfiguration.NumberOfAttemptsToRetryToProcessTheMessage);
            foreach (var message in failedMessages)
            {
                try
                {
                    this.messageBus.Publish(message);
                }
                catch (Exception ex)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { Logger.CORRELATIONID, message.Id } };
                    this.logger.LogErrorMessageWithException(string.Format("RetryFailedMessages : {0}", this.GetType()), "Publish", ex, parameters);
                }
            }
            this.Reset();
        }
    }
}
