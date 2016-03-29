using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Workflow.Origination.Credit
{
    public class SendResubMailCommandHandler : IHandlesDomainServiceCommand<SendResubMailCommand>
    {
        private IApplicationRepository applicationRepository;
        private IMessageService messageService;

        public SendResubMailCommandHandler(IApplicationRepository applicationRepository, IMessageService messageService)
        {
            this.applicationRepository = applicationRepository;
            this.messageService = messageService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendResubMailCommand command)
        {
            IApplication app = applicationRepository.GetApplicationByKey(command.ApplicationKey);

            string emailAddress = "regqueries@sahomeloans.com";
            string emailSubject = "Resub declined by credit";
            StringBuilder body = new StringBuilder();
            body.AppendFormat(" The following application {0} , accountkey {1} has been declined by Credit and is now with the consultant to archive or motivate. Please follow up and inform the attorney.", command.ApplicationKey, app.ReservedAccount.Key);

            messageService.SendEmailInternal(SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo,
                                             emailAddress,
                                             null,
                                             null,
                                             emailSubject,
                                             body.ToString());
        }
    }
}