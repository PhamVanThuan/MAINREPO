using SAHL.Core.Exchange;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers.Mailbox;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryHandlers
{
    public class WaitForMessagesToBeDeliveredQueryHandler : IServiceQueryHandler<WaitForMessagesToBeDeliveredQuery>
    {
        private IMailboxManager mailboxManager;

        public WaitForMessagesToBeDeliveredQueryHandler(IMailboxManager mailboxManager)
        {
            this.mailboxManager = mailboxManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleQuery(WaitForMessagesToBeDeliveredQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            mailboxManager.OpenMailboxConnection();
            if (query.TimeoutSeconds > 0)
            {
                query.Result = new ServiceQueryResult<IMailMessage>(mailboxManager.WaitForMessagesToBeDelivered(query.TimeoutSeconds));
            }
            else
            {
                //user default timeout
                query.Result = new ServiceQueryResult<IMailMessage>(mailboxManager.WaitForMessagesToBeDelivered());
            }
            return messages;
        }
    }
}