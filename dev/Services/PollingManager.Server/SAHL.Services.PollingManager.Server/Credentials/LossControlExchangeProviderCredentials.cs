using SAHL.Core.Exchange;
using System.Collections.Specialized;

namespace SAHL.Services.PollingManager.Credentials
{
    public class LossControlExchangeProviderCredentials : IExchangeProviderCredentials
    {
        private NameValueCollection nameValueCollection;

        public LossControlExchangeProviderCredentials(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
        }

        public string ServerUrl
        {
            get
            {
                string value = nameValueCollection["MailServerUrl"];
                return value ?? string.Empty;
            }
        }

        public string UserName
        {
            get
            {
                string value = nameValueCollection["MailUserName"];
                return value ?? string.Empty;
            }
        }

        public string Password
        {
            get
            {
                string value = nameValueCollection["MailPassword"];
                return value ?? string.Empty;
            }
        }

        public string MailBox
        {
            get
            {
                string value = nameValueCollection["MailBox"];
                return value ?? string.Empty;
            }
        }

        public string ArchiveFolder
        {
            get
            {
                string value = nameValueCollection["MailArchiveFolder"];
                return value ?? string.Empty;
            }
        }
    }
}