using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.ExternalServices.Notification;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.ExternalServicesSpecs.NotificationSpecs.SMSNotificationServiceSpecs.NotifyRecipientSpecs
{
    public class when_notifications_disabled : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection result, expectedResult;
        private static List<Recipient> recipients;
        private static string message;

        Establish context = () => 
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(false);
            handlerProvider = An<IHttpMessageHandlerProviderService>();

            service = new SMSNotificationService(serviceConfig, handlerProvider);

            recipients = new List<Recipient>(){ new Recipient() };
            message = String.Empty;

            expectedResult = SystemMessageCollection.Empty();
        };

        Because of = () =>
        {
            result = service.NotifyRecipients(recipients, message);
        };

        It should_return_empty_message_collection = () =>
        {
            result.ShouldBeLike(expectedResult);
        };
    }
}
