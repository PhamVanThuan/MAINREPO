using SAHL.Core.Exchange;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class ArchiveEmailCommand : ServiceCommand, IFrontEndTestCommand
    {
        public IMailMessage MailMessage { get; protected set; }

        public ArchiveEmailCommand(IMailMessage mailMessage)
        {
            this.MailMessage = mailMessage;
        }
    }
}