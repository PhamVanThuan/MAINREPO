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
    public class when_notifications_disabled : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection result, expectedResult;
        private static List<Recipient> recipients;
        private static string message;

        private Establish context = () =>
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(false);
            handlerProvider = An<IHttpMessageHandlerProviderService>();

            service = new SMSNotificationService(serviceConfig, handlerProvider);

            recipients = new List<Recipient>() { new Recipient("0764486581") };
            message = String.Empty;

            expectedResult = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            result = service.NotifyRecipients(recipients, message);
        };

        private It should_return_empty_message_collection = () =>
        {
            result.ShouldBeLike(expectedResult);
        };
    }
}