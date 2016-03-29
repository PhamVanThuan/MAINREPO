using SAHL.Core.Exchange;
using System.Collections.Generic;

namespace SAHL.Services.FrontEndTest.Managers.Mailbox
{
    public interface IMailboxManager
    {
        bool OpenMailboxConnection(IExchangeProviderCredentials credentials);

        bool OpenMailboxConnection();

        bool MoveMessage(IMailMessage mailMessage);

        bool SendMessage(IMailMessage mailMessage);

        List<IMailMessage> FetchMessages();

        IEnumerable<IMailMessage> WaitForMessagesToBeDelivered();

        IEnumerable<IMailMessage> WaitForMessagesToBeDelivered(int timeoutSeconds);
    }
}