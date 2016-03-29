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
    public class when_multiple_applicants : WithFakes
    {
        private static INotificationService service;
        private static ISMSNotificationServiceConfiguration serviceConfig;
        private static IHttpMessageHandlerProviderService handlerProvider;
        private static ISystemMessageCollection result, expectedResult;
        private static List<Recipient> recipients;
        private static Recipient recipient1, recipient2;
        private static string message;
        private static FakeHttpMessageHandler messageHandler;

        Establish context = () =>
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(true);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Return(messageHandler);

            recipient1 = new Recipient();
            recipient1.CellPhoneNumber = "0821111111";
            
            recipient2 = new Recipient();
            recipient2.CellPhoneNumber = "0822222222";

            message = Guid.NewGuid().ToString();

            recipients = new List<Recipient>() { recipient1, recipient2 };

            service = new SMSNotificationService(serviceConfig, handlerProvider);
        };

        Because of = () =>
        {
            service.NotifyRecipients(recipients, message);
        };

        It should_submit_a_request_to_the_sms_gateway = () =>
        {
            messageHandler.LastMessageSent.ShouldNotBeNull();
            messageHandler.RequestContent.ShouldContain(recipient1.CellPhoneNumber);
            messageHandler.RequestContent.ShouldContain(recipient2.CellPhoneNumber);
            messageHandler.RequestContent.ShouldContain(message);
        };
    }
}
