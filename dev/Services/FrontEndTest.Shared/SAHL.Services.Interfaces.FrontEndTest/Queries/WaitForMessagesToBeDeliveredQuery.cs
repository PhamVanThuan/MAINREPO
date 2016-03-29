using SAHL.Core.Exchange;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class WaitForMessagesToBeDeliveredQuery : ServiceQuery<IMailMessage>, IFrontEndTestQuery
    {
        public int TimeoutSeconds { get; private set; }

        public WaitForMessagesToBeDeliveredQuery(int timeoutSeconds = -1)
        {
            this.TimeoutSeconds = timeoutSeconds;
        }
    }
}