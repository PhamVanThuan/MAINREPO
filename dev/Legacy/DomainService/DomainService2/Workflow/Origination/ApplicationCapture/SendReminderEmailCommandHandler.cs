using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class SendReminderEmailCommandHandler : IHandlesDomainServiceCommand<SendReminderEmailCommand>
    {
        private IX2Repository x2Repository;
        private IMessageService messageService;

        public SendReminderEmailCommandHandler(IX2Repository x2Repository, IMessageService messageService)
        {
            this.x2Repository = x2Repository;
            this.messageService = messageService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendReminderEmailCommand command)
        {
            string emailAddress = x2Repository.GetCurrentConsultantEmailAddress(command.InstanceID);

            if (!string.IsNullOrEmpty(emailAddress))
            {
                string emailBodyFormat = "This is a reminder that Application :{0} has not been completed. You will be reminded in another 15 days.";
                string emailSubject = "Auto 30 day reminder";

                StringBuilder emailBody = new StringBuilder();
                emailBody.AppendFormat(emailBodyFormat, command.ApplicationKey);
                emailBody.AppendLine();

                messageService.SendEmailInternal(SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo,
                                                 emailAddress,
                                                 null,
                                                 null,
                                                 emailSubject,
                                                 emailBody.ToString());
            }
        }
    }
}