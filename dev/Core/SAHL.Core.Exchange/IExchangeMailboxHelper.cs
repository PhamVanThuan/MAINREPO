using SAHL.Core.Exchange.Provider;
using System.Collections.Generic;

namespace SAHL.Core.Exchange
{
    public interface IExchangeMailboxHelper
    {
        List<IMailMessage> EmailMessages { get; }
        bool MoveMessage(string archiveFolder, IMailMessage mailMessage);
        bool SendMessage(IMailMessage mailMessage);
        bool UpdateMessages();
    }
}