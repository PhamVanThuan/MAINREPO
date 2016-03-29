using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.RequestPublisher
{
    public class RequestPublisherManager : IRequestPublisherManager
    {
        private IMessageBus bus;
        private ILogger logger;
        private ILoggerSource loggerSource;
        private IRequestPublisherDataManager capitecRequestRepository;
        private IConfigurationProvider capitecConfigurationProvider;
        private ILookupManager lookupService;

        public RequestPublisherManager(ILookupManager lookupService, IMessageBus bus, ILogger logger, IRequestPublisherDataManager capitecRequestRepository, IConfigurationProvider capitecConfigurationProvider, ILoggerSource loggerSource)
        {
            this.lookupService = lookupService;
            this.bus = bus;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.capitecRequestRepository = capitecRequestRepository;
            this.capitecConfigurationProvider = capitecConfigurationProvider;
        }

        public bool PublishWithRetry(SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication)
        {
            var messageId = this.lookupService.GenerateCombGuid();
            var createApplicationRequest = new CreateCapitecApplicationRequest(capitecApplication, messageId);

            capitecRequestRepository.AddGenericMessage(capitecApplication);

            for (int i = 0; i < capitecConfigurationProvider.NumberOfTimesToRetryCreateApplication; i++)
            {
                try
                {
                    bus.Publish((dynamic)createApplicationRequest);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogErrorWithException(this.loggerSource, "SystemUser", "PublishWithRetry", "Failed to publish message", ex);
                }
            }

            capitecRequestRepository.AddPublishMessageFailure(capitecApplication);

            return false;
        }
    }
}