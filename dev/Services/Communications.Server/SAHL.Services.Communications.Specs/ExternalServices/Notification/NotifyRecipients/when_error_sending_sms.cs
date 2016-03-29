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
    public class when_error_sending_sms : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection result, expectedResult;
        private static List<Recipient> recipients;
        private static Recipient recipient;
        private static string message;
        private static FakeHttpMessageHandler messageHandler;
        private static string testRecipientNumber = "0720000000";

        private Establish context = () =>
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(false);
            serviceConfig.WhenToldTo(x => x.TestRecipientNumber).Return(testRecipientNumber);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Throw(new Exception());

            recipient = new Recipient("0823334455");
            message = Guid.NewGuid().ToString();

            recipients = new List<Recipient>() { recipient };

            service = new SMSNotificationService(serviceConfig, handlerProvider);

            expectedResult = SystemMessageCollection.Empty();
            expectedResult.AddMessage(new SystemMessage("Unable to send SMS to client. An error was encountered.", SystemMessageSeverityEnum.Info));
        };

        private Because of = () =>
        {
            result = service.NotifyRecipients(recipients, message);
        };

        private It should_add_message_to_message_collection = () =>
        {
            result.ShouldBeLike(expectedResult);
        };
    }
}