using SAHL.Core.Exchange;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class ExchangeProviderCredentialsModel : IExchangeProviderCredentials
    {
        public ExchangeProviderCredentialsModel(string archiveFolder, string mailBox, string password, string serverUrl, string userName)
        {
            this.ArchiveFolder = archiveFolder;
            this.MailBox = mailBox;
            this.Password = password;
            this.ServerUrl = serverUrl;
            this.UserName = userName;
        }

        public string ArchiveFolder
        {
            get;
            protected set;
        }

        public string MailBox
        {
            get;
            protected set;
        }

        public string Password
        {
            get;
            protected set;
        }

        public string ServerUrl
        {
            get;
            protected set;
        }

        public string UserName
        {
            get;
            protected set;
        }
    }
}