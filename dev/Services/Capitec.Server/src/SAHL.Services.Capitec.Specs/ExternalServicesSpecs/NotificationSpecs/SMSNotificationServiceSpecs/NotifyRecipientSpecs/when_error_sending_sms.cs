using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.ExternalServices.Notification;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ExternalServiceModels.Notification;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.ExternalServicesSpecs.NotificationSpecs.SMSNotificationServiceSpecs.NotifyRecipientSpecs
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

        Establish context = () =>
        {
            serviceConfig = An<ISMSNotificationServiceConfiguration>();
            serviceConfig.WhenToldTo(x => x.EnableNotifications).Return(true);
            serviceConfig.WhenToldTo(x => x.UseRecipientNumber).Return(false);
            serviceConfig.WhenToldTo(x => x.TestRecipientNumber).Return(testRecipientNumber);
            serviceConfig.WhenToldTo(x => x.AppLinkUploadUrl).Return("http://www.sahl.com/");
            messageHandler = new FakeHttpMessageHandler();

            handlerProvider = An<IHttpMessageHandlerProviderService>();
            handlerProvider.WhenToldTo(x => x.GetMessageHandler()).Throw(new Exception());

            recipient = new Recipient();
            recipient.CellPhoneNumber = "0823334455";

            message = Guid.NewGuid().ToString();

            recipients = new List<Recipient>() { recipient };

            service = new SMSNotificationService(serviceConfig, handlerProvider);

            expectedResult = SystemMessageCollection.Empty(); 
            expectedResult.AddMessage(new SystemMessage("Unable to send SMS to client.", SystemMessageSeverityEnum.Info));
        };

        Because of = () =>
        {
            result = service.NotifyRecipients(recipients, message);
        };

        It should_add_message_to_message_collection = () =>
        {
            result.ShouldBeLike(expectedResult);
        };
    }
}
