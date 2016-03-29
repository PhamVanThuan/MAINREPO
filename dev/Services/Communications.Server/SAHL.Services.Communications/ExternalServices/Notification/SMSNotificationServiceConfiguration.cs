using System;
using System.Configuration;
namespace SAHL.Services.Communications.ExternalServices.Notification
{
    public class SMSNotificationServiceConfiguration : ISMSNotificationServiceConfiguration
    {
        public SMSNotificationServiceConfiguration()
        {
            this.AffiliateCode = ConfigurationManager.AppSettings["AffiliateCode"].ToString();
            this.AuthenticationCode = ConfigurationManager.AppSettings["AuthenticationCode"].ToString();
            this.MessageType = ConfigurationManager.AppSettings["MessageType"].ToString();
            this.AppLinkUploadUrl = ConfigurationManager.AppSettings["AppLinkUploadUrl"].ToString();
            this.StartHour = ConfigurationManager.AppSettings["StartHour"].ToString();
            this.EndHour = ConfigurationManager.AppSettings["EndHour"].ToString();
            this.UseRecipientNumber = Convert.ToBoolean(ConfigurationManager.AppSettings["UseRecipientNumber"].ToString());
            this.TestRecipientNumber = ConfigurationManager.AppSettings["TestRecipientNumber"].ToString();
            this.EnableNotifications = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotifications"]);
        }

        public string AffiliateCode { get; protected set; }

        public string AuthenticationCode { get; protected set; }

        public string MessageType { get; protected set; }

        public string AppLinkUploadUrl { get; protected set; }

        public string StartHour { get; protected set; }

        public string EndHour { get; protected set; }

        public bool UseRecipientNumber { get; protected set; }

        public string TestRecipientNumber { get; protected set; }

        public bool EnableNotifications { get; protected set; }
    }
}