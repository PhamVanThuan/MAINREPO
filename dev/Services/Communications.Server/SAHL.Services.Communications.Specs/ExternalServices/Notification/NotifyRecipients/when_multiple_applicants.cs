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
    public class when_multiple_applicants : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection result;
        private static List<Recipient> recipients;
        private static Recipient recipient1, recipient2;
        private static string message;
        private static FakeHttpMessageHandler messageHandler;

        private Establish context = () =>
        {
            result = SystemMessageCollection.Empty();
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(true);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Return(messageHandler);

            recipient1 = new Recipient("0821111111");

            recipient2 = new Recipient("0822222222");

            message = Guid.NewGuid().ToString();

            recipients = new List<Recipient>() { recipient1, recipient2 };

            service = new SMSNotificationService(serviceConfig, handlerProvider);
        };

        private Because of = () =>
        {
            result = service.NotifyRecipients(recipients, message);
        };

        private It should_not_return_error_messages = () =>
        {
            result.ErrorMessages().ShouldBeEmpty();
        };

        private It should_submit_a_request_to_the_sms_gateway = () =>
        {
            messageHandler.LastMessageSent.ShouldNotBeNull();
            messageHandler.RequestContent.ShouldContain(message);
        };

        private It should_send_to_the_first_recipient = () =>
        {
            messageHandler.RequestContent.ShouldContain(recipient1.CellPhoneNumber);
        };

        private It should_send_to_the_second_recipient = () =>
        {
            messageHandler.RequestContent.ShouldContain(recipient2.CellPhoneNumber);
        };

    }
}