using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.ExternalServices.Notification;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.ExternalServicesSpecs.NotificationSpecs.SMSNotificationServiceSpecs.NotifyRecipientSpecs
{
    public class when_use_recipient_number_false : WithFakes
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

        Establish context = () =>
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(false);
            serviceConfig.WhenToldTo(x => x.TestRecipientNumber).Return(testRecipientNumber);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Return(messageHandler);

            recipient = new Recipient();
            recipient.CellPhoneNumber = "0823334455";

            message = Guid.NewGuid().ToString();

            recipients = new List<Recipient>() { recipient };

            service = new SMSNotificationService(serviceConfig, handlerProvider);
        };

        Because of = () =>
        {
            service.NotifyRecipients(recipients, message);
        };

        It should_submit_a_request_to_the_sms_gateway = () =>
        {
            messageHandler.LastMessageSent.ShouldNotBeNull();
            messageHandler.RequestContent.ShouldContain(testRecipientNumber);
            messageHandler.RequestContent.ShouldNotContain(recipient.CellPhoneNumber);
            messageHandler.RequestContent.ShouldContain(message);
        };
    }
}
