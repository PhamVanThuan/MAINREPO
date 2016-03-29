using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class SendEmailCommand : ServiceCommand, IFrontEndTestCommand
    {
        public IMailMessage MailMessage { get; protected set; }

        public SendEmailCommand(MailMessage mailMessage)
        {
            this.MailMessage = mailMessage;
        }
    }
}