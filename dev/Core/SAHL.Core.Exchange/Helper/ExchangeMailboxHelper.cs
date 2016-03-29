using System.Collections.Generic;

namespace SAHL.Core.Exchange.Provider
{
    public class ExchangeMailboxHelper : IExchangeMailboxHelper
    {
        private List<IMailMessage> messages;

        private IExchangeProvider exchangeProvider { get; set; }

        public IMailMessage CurrentMessage { get; private set; }

        public List<IMailMessage> EmailMessages { get { return messages; } }

        private IExchangeProviderCredentials exchangeCredentials;

        public ExchangeMailboxHelper(IExchangeProvider exchangeProvider)
        {
            messages = new List<IMailMessage>();
            this.exchangeProvider = exchangeProvider;
            this.exchangeCredentials = exchangeProvider.ExchangeProviderCredentials;
        }

        public bool UpdateMessages()
        {
            if (!exchangeProvider.OpenMailboxConnection()) { return false; }
            messages = exchangeProvider.FetchAllMessagesFromInbox();

            return true;
        }

        public bool MoveMessage(string archiveFolder, IMailMessage mailMessage)
        {
            return exchangeProvider.MoveMailMessageToFolder(mailMessage, archiveFolder);
        }

        public bool SendMessage(IMailMessage mailMessage)
        {
            return exchangeProvider.SendMessage(mailMessage);
        }
    }
}