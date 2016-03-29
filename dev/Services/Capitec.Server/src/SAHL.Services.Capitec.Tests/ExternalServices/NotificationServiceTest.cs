//using NUnit.Framework;
//using SAHL.Services.Capitec.ExternalServices.Notification;
//using SAHL.Services.Interfaces.Capitec.Common;
//using SAHL.Services.Interfaces.Communications.ExternalServiceModels.Notification;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SAHL.Core.Services;
//using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;

//namespace SAHL.Services.Capitec.Tests.ExternalServices
//{
//    [TestFixture]
//    public class NotificationServiceTest
//    {
//        [Ignore("live sms post via grapevine")]
//        [Test]
//        public void TestNotificationService() 
//        { 
//            var smsNotificationServiceConfiguration = new SMSNotificationServiceConfiguration();
//            var messageHandler = new SAHL.Core.Services.HttpMessageHandlerProviderService();
//            INotificationService notificationService = new SMSNotificationService(smsNotificationServiceConfiguration, messageHandler);
//            var recipients = new List<Recipient>();
//            var applicationId = 54545;
//            recipients.Add(new Recipient()
//            {
//                    CellPhoneNumber="12345678"
//            });
//            notificationService.NotifyRecipients(recipients, Notifications.UpdateApplicationRecievedNotification(applicationId));
//        }
//    }
//}
