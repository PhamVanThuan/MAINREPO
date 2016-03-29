using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace SAHL.Services.Communications.ExternalServices.Notification
{
    public class SMSNotificationService : INotificationService
    {
        private ISMSNotificationServiceConfiguration smsNotificationServiceConfiguration;
        private IHttpMessageHandlerProviderService handlerProvider;

        public SMSNotificationService(ISMSNotificationServiceConfiguration smsNotificationServiceConfiguration, IHttpMessageHandlerProviderService handlerProvider)
        {
            this.smsNotificationServiceConfiguration = smsNotificationServiceConfiguration;
            this.handlerProvider = handlerProvider;
        }

        public ISystemMessageCollection NotifyRecipients(IEnumerable<Recipient> recipients, string message)
        {
            ISystemMessageCollection messageCollection = SystemMessageCollection.Empty();
            try
            {
                if (this.smsNotificationServiceConfiguration.EnableNotifications)
                {
                    List<string> msisdnCollection = new List<string>();
                    foreach (var recipient in recipients)
                    {
                        if (this.smsNotificationServiceConfiguration.UseRecipientNumber)
                        {
                            msisdnCollection.Add(recipient.CellPhoneNumber);
                        }
                        else
                        {
                            msisdnCollection.Add(this.smsNotificationServiceConfiguration.TestRecipientNumber);
                        }
                    }
                    SendSMS(msisdnCollection, message);
                }
            }
            catch (Exception)
            {
                messageCollection.AddMessage(new SystemMessage("Unable to send SMS to client. An error was encountered.", SystemMessageSeverityEnum.Info));
            }
            return messageCollection;
        }

        private void SendSMS(List<string> msisdnCollection, string message)
        {
            var transmitDateTime = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");

            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                //start
                writer.WriteStartElement("gviSmsMessage");
                writer.WriteElementString("affiliateCode", this.smsNotificationServiceConfiguration.AffiliateCode);
                writer.WriteElementString("authenticationCode", this.smsNotificationServiceConfiguration.AuthenticationCode);
                writer.WriteElementString("messageType", this.smsNotificationServiceConfiguration.MessageType);

                // start - recipientList
                writer.WriteStartElement("recipientList");
                writer.WriteElementString("message", message);

                foreach (var msisdn in msisdnCollection)
                {
                    //start - recipient
                    writer.WriteStartElement("recipient");
                    writer.WriteElementString("msisdn", msisdn);
                    writer.WriteEndElement();
                    //end - recipient
                }

                writer.WriteEndElement();
                // end - recipientList

                //start - transmissionRules
                writer.WriteStartElement("transmissionRules");

                writer.WriteElementString("transmitDateTime", transmitDateTime);

                //start - transmitPeriod
                writer.WriteStartElement("transmitPeriod");

                writer.WriteElementString("startHour", this.smsNotificationServiceConfiguration.StartHour);
                writer.WriteElementString("endHour", this.smsNotificationServiceConfiguration.EndHour);

                //end - transmitPeriod
                writer.WriteEndElement();

                //end - transmissionRules
                writer.WriteEndElement();

                //end
                writer.WriteEndElement();
                writer.Flush();
            }

            using (HttpClient client = new HttpClient(handlerProvider.GetMessageHandler()))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, this.smsNotificationServiceConfiguration.AppLinkUploadUrl);
                request.Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/xml");
                var response = client.SendAsync(request);
                response.Result.Content.ReadAsStringAsync();
            }
        }
    }
}