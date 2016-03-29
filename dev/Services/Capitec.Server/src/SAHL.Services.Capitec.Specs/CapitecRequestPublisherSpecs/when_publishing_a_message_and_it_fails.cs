using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Managers.RequestPublisher;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;

using System;

namespace SAHL.Services.Capitec.Specs.CapitecRequestPublisherSpecs
{
    public class when_publishing_a_message_and_it_fails : WithFakes
    {
        private static IRequestPublisherManager capitecRequestPublisher;
        private static IMessageBus messageBus;
        private static IConfigurationProvider capitecConfigurationProvider;
        private static IRequestPublisherDataManager capitecRequestRepository;
        private static NewPurchaseApplication capitecApplication;
        private static ILookupManager lookupService;
        private static Guid messageId;

        private Establish context = () =>
        {
            messageId = Guid.NewGuid();
            messageBus = An<IMessageBus>();
            lookupService = An<ILookupManager>();
            capitecConfigurationProvider = An<IConfigurationProvider>();
            capitecRequestRepository = An<IRequestPublisherDataManager>();
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(messageId);
            capitecApplication = new NewPurchaseApplication(1, 1, DateTime.Now, null, null, 1, null, null);
            var request = new CreateCapitecApplicationRequest(capitecApplication, messageId);

            capitecConfigurationProvider.WhenToldTo(x => x.NumberOfTimesToRetryCreateApplication).Return(3);
            capitecRequestPublisher = new RequestPublisherManager(lookupService, messageBus, An<ILogger>(), capitecRequestRepository, capitecConfigurationProvider, An<ILoggerSource>());
            messageBus.WhenToldTo(x => x.Publish<CreateCapitecApplicationRequest>(Param.IsAny<CreateCapitecApplicationRequest>())).Throw(new Exception());
        };

        private Because of = () =>
        {
            Catch.Exception(() => { capitecRequestPublisher.PublishWithRetry(capitecApplication); });
        };

        private It should_retry_as_many_times_as_specified_in_configuration = () =>
        {
            messageBus.WasToldTo(x => x.Publish(Param<CreateCapitecApplicationRequest>.Matches(m => m.CapitecApplication == capitecApplication && m.Id == messageId))).Times(capitecConfigurationProvider.NumberOfTimesToRetryCreateApplication);
        };

        private It should_store_the_capitec_application_request = () =>
        {
            capitecRequestRepository.WasToldTo(x => x.AddGenericMessage(capitecApplication));
        };

        private It should_store_the_failed_create_sa_homeloans_application_message = () =>
        {
            capitecRequestRepository.WasToldTo(x => x.AddPublishMessageFailure(capitecApplication));
        };
    }
}