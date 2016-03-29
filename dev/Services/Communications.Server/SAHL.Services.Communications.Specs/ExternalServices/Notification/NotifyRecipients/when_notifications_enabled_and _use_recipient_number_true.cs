using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.ExternalServices.Notification;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Communications.Specs.ExternalServices.Notification.NotifyRecipients
{
    public class when_notifications_enabled_and_use_recipient_number_true : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection results;
        private static List<Recipient> recipients;
        private static Recipient recipient;
        private static string message;
        private static FakeHttpMessageHandler messageHandler;

        private Establish context = () =>
        {
            results = SystemMessageCollection.Empty();
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(true);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Return(messageHandler);

            recipient = new Recipient("0823334455");
            message = Guid.NewGuid().ToString();
            recipients = new List<Recipient>() { recipient };
            service = new SMSNotificationService(serviceConfig, handlerProvider);
        };

        private Because of = () =>
        {
            results = service.NotifyRecipients(recipients, message);
        };

        private It should_not_return_errors = () =>
        {
            results.ErrorMessages().ShouldBeEmpty();
        };

        private It should_submit_a_request_to_the_sms_gateway = () =>
        {
            messageHandler.LastMessageSent.ShouldNotBeNull();
            messageHandler.RequestContent.ShouldContain(recipient.CellPhoneNumber);
            messageHandler.RequestContent.ShouldContain(message);
        };
    }
}