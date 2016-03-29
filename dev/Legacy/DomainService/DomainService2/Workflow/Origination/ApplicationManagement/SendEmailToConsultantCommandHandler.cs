using System.Data;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendEmailToConsultantForQueryCommandHandler : IHandlesDomainServiceCommand<SendEmailToConsultantForQueryCommand>
    {
        protected IReasonRepository reasonRepository;
        protected IX2Repository x2Repository;
        protected IMessageService messageService;

        public SendEmailToConsultantForQueryCommandHandler(IReasonRepository reasonRepository, IX2Repository x2Repository, IMessageService messageService)
        {
            this.reasonRepository = reasonRepository;
            this.x2Repository = x2Repository;
            this.messageService = messageService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendEmailToConsultantForQueryCommand command)
        {
            IInstance instance = x2Repository.GetInstanceByKey(command.InstanceID);
            DataTable dt = x2Repository.GetCurrentConsultantAndAdmin(command.InstanceID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string toAddress = dt.Rows[i]["EMailAddress"].ToString();
                if (string.IsNullOrEmpty(toAddress))
                    continue;

                // use the reasontypegroup and the gen key to get the common reasons to send to consultant.
                IReadOnlyEventList<IReason> reasons = reasonRepository.GetReasonByGenericKeyAndReasonGroupTypeKey(command.GenericKey, command.ReasonGroupTypeKey);
                StringBuilder sbBody = new StringBuilder();
                sbBody.AppendFormat("Reasons for QA Query for Offer:{0} Client:{1}", command.GenericKey, instance.Subject);
                sbBody.AppendLine();
                foreach (IReason r in reasons)
                {
                    sbBody.AppendFormat("{0} - {1}", r.ReasonDefinition.ReasonDescription.Description, r.Comment);
                    sbBody.AppendLine();
                }

                string fromAddress = SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo;
                string cc = string.Empty;
                string bcc = string.Empty;
                string subject = "QA Query from Halo";
                messageService.SendEmailInternal(fromAddress, toAddress, cc, bcc, subject, sbBody.ToString());
            }
        }
    }
}