using SAHL.Services.Interfaces.FrontEndTest.Models;
using System.Configuration;

namespace SAHL.Testing.Services.Tests.Helpers
{
    public static class MailboxHelper
    {
        public static string legalfeeInvoicesMailBox = ConfigurationManager.AppSettings["LegalfeeInvoicesMailBox"];
        public static string haloMailBox = ConfigurationManager.AppSettings["HaloMailBox"];

        public static ExchangeProviderCredentialsModel testExchangeProviderCredentials = new ExchangeProviderCredentialsModel(
            ConfigurationManager.AppSettings["MailArchiveFolder"],
            ConfigurationManager.AppSettings["MailBox"],
            ConfigurationManager.AppSettings["MailPassword"],
            ConfigurationManager.AppSettings["MailServerUrl"],
            ConfigurationManager.AppSettings["MailUserName"]);
    }
}