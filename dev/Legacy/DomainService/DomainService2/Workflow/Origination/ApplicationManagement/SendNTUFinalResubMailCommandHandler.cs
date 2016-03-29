using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendNTUFinalResubMailCommandHandler : IHandlesDomainServiceCommand<SendNTUFinalResubMailCommand>
    {
        IApplicationRepository applicationRepository;
        IMessageService messageService;

        public SendNTUFinalResubMailCommandHandler(IApplicationRepository applicationRepository, IMessageService messageService)
        {
            this.applicationRepository = applicationRepository;
            this.messageService = messageService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendNTUFinalResubMailCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);

            StringBuilder body = new StringBuilder();
            body.AppendFormat(" The following application {0} , accountkey {1} has been declined by Credit and has been Decline Finalised by the consultant. ", application.Key, application.ReservedAccount.Key);

            string fromAddress = SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo;
            string toAddress = SAHL.Common.ApplicationManagement.EmailAddresses.ToRegQueries;
            string cc = string.Empty;
            string bcc = string.Empty;
            string subject = "Resub declined by credit";

            // sends mail directly - doesn't insert into clientEmail table
            messageService.SendEmailInternal(fromAddress, toAddress, cc, bcc, subject, body.ToString());
        }
    }
}