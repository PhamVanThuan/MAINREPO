using SAHL.Common.Logging;
using SAHL.Communication;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public class MessageQueueHandler<TMessage> : IQueuedHandler<TMessage>
        where TMessage : class, IBatchMessage
    {
        private readonly IDiposableMessageBus messageBus;
        private ICancellationNotifier cancellationNotifier;
        private CancellationToken cancellationToken;
        private ILogger logger;
        private IRepository repository;
        private IMessageProcessor<TMessage> messageProcessor;
        private IBatchServiceConfiguration messageQueueHandlerConfiguration;
        private IMessageRetryService<TMessage> messageRetryService;

        public MessageQueueHandler(IRepository repository, IMessageProcessor<TMessage> messageProcessor, IBatchServiceConfiguration messageQueueHandlerConfiguration, ILogger logger, IDiposableMessageBus messageBus, ICancellationNotifier cancellationNotifier, IMessageRetryService<TMessage> messageRetryService)
        {
            this.messageBus = messageBus;
            this.cancellationNotifier = cancellationNotifier;
            this.logger = logger;
            this.repository = repository;
            this.messageProcessor = messageProcessor;
            this.messageQueueHandlerConfiguration = messageQueueHandlerConfiguration;
            this.messageRetryService = messageRetryService;
            messageBus.Subscribe<TMessage>(HandleMessage);
            cancellationToken = cancellationNotifier.GetTokenInstance();
        }

        public void HandleMessage(TMessage message)
        {
            this.cancellationToken.ThrowIfCancellationRequested();
            this.messageRetryService.Reset();
            if (message.Id == 0)
            {
                this.repository.Save(message);
            }

            for (int i = 0; i < messageQueueHandlerConfiguration.NumberOfTimesToRetryToProcessTheMessage; i++)
            {
                try
                {
                    var status = this.messageProcessor.Process(message);
                    var genericStatus = status ? GenericStatuses.Complete : GenericStatuses.Unsuccessful;
                    this.repository.Update(message, genericStatus);
                    return;
                }
                catch (Exception ex)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { Logger.CORRELATIONID, message.Id } };
                    this.logger.LogErrorMessageWithException(string.Format("MessageQueueHandler : {0}", this.GetType()), "HandleMessage", ex, parameters);
                }
            }
            message.FailureCount++;
            this.repository.Update(message, GenericStatuses.Failed);
        }

        public void Stop()
        {
            
        }

        public void Start()
        {
            this.messageRetryService.Start();
        }
    }
}