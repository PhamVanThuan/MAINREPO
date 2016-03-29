using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Exchange.WebServices.Data;
using SAHL.Core.Exchange.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Core.Exchange.Specs.ExchangeProviderSpecs
{
    public abstract class WithExchangeProviderFakes : WithFakes
    {
        protected static ExchangeProvider exchangeProvider;
        protected static ExchangeProviderCredentials credentials;
        protected static ExchangeService exchangeService;
        private static string mailBox;
        private static string password;
        private static string serverUrl;
        private static string userName;
        private const string archiveFolder = "TestArchive";

        private Establish context = () =>
        {
            mailBox = "halouser@sahomeloans.com";
            password = "Natal123";
            userName = @"SAHL\HaloUser";
            serverUrl = "https://sahl-mb01/ews";
            exchangeService = new ExchangeService();
            credentials = new ExchangeProviderCredentials();
            credentials.MailBox = mailBox;
            credentials.Password = password;
            credentials.UserName = userName;
            credentials.ServerUrl = serverUrl;
            exchangeProvider = new ExchangeProvider(exchangeService, credentials);
        };

        protected static IEnumerable<IMailMessage> WaitForMessageToBeDelivered(IMailMessage mailMessage)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<IMailMessage> mailMessages;
            while (sw.Elapsed < TimeSpan.FromSeconds(60))
            {
                mailMessages = exchangeProvider.FetchAllMessagesFromInbox();
                var messages = mailMessages.Where(x => x.Subject.Equals(mailMessage.Subject)).AsEnumerable();
                if (messages.Count() > 0)
                {
                    sw.Stop();
                    return messages;
                }
            }
            return Enumerable.Empty<IMailMessage>();
        }

        protected static void CreateArchiveFolder()
        {
            if (exchangeProvider == null)
            {
                throw new Exception("This method needs be used in context of a specification.");
            }
            exchangeProvider.OpenMailboxConnection();
            var folder = exchangeProvider.ExchangeService
                    .FindFolders(WellKnownFolderName.Inbox, new SearchFilter.ContainsSubstring(FolderSchema.DisplayName, archiveFolder), new FolderView(1)).Folders.FirstOrDefault();
            if (folder == null)
            {
                exchangeProvider.CreateFolderOffInbox(archiveFolder);
            }
        }

        protected static void MoveMessageToArchive(IMailMessage message)
        {
            CreateArchiveFolder();
            exchangeProvider.OpenMailboxConnection();
            exchangeProvider.MoveMailMessageToFolder(message, archiveFolder);
        }
    }
}