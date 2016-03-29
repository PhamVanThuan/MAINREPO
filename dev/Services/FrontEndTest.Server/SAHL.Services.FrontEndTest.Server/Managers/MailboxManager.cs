using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Mailbox
{
    public class MailboxManager : IMailboxManager
    {
        private IExchangeProvider exchangeProvider;
        private int timeout;
        public IExchangeProviderCredentials exchangeProviderCredentials;

        public MailboxManager(NameValueCollection nameValueCollection)
        {
            exchangeProviderCredentials = new ExchangeProviderCredentials()
            {
                ArchiveFolder = nameValueCollection["MailArchiveFolder"],
                MailBox = nameValueCollection["MailBox"],
                Password = nameValueCollection["MailPassword"],
                ServerUrl = nameValueCollection["MailServerUrl"],
                UserName = nameValueCollection["MailUserName"]
            };
            timeout = Convert.ToInt32(nameValueCollection["WaitForEmailTimeout"]);
        }

        public bool OpenMailboxConnection(IExchangeProviderCredentials credentials)
        {
            this.exchangeProviderCredentials = credentials;
            var exchangeService = ExchangeServiceFactory.Create(exchangeProviderCredentials);
            exchangeProvider = new ExchangeProvider(exchangeService, exchangeProviderCredentials);
            return exchangeProvider.OpenMailboxConnection();
        }

        public bool OpenMailboxConnection()
        {
            return OpenMailboxConnection(this.exchangeProviderCredentials);
        }

        public bool MoveMessage(IMailMessage mailMessage)
        {
            return exchangeProvider.MoveMailMessageToFolder(mailMessage, exchangeProvider.ExchangeProviderCredentials.ArchiveFolder);
        }

        public bool SendMessage(IMailMessage mailMessage)
        {
            mailMessage.From = exchangeProviderCredentials.MailBox;
            return exchangeProvider.SendMessage(mailMessage);
        }

        public List<IMailMessage> FetchMessages()
        {
            return exchangeProvider.FetchAllMessagesFromInbox();
        }

        public IEnumerable<IMailMessage> WaitForMessagesToBeDelivered()
        {
            return WaitForMessagesToBeDelivered(timeout);
        }

        public IEnumerable<IMailMessage> WaitForMessagesToBeDelivered(int timeoutseconds)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<IMailMessage> mailMessages;
            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutseconds))
            {
                mailMessages = exchangeProvider.FetchAllMessagesFromInbox();
                if (mailMessages.Count() > 0)
                {
                    sw.Stop();
                    return mailMessages;
                }
            }
            return Enumerable.Empty<IMailMessage>();
        }
    }
}