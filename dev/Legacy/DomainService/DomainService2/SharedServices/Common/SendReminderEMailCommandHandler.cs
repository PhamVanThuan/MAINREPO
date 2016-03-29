using System.Data;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class SendReminderEMailCommandHandler : IHandlesDomainServiceCommand<SendReminderEMailCommand>
    {
        private IMessageService messageService;
        private ICastleTransactionsService service;

        public SendReminderEMailCommandHandler(IMessageService messageService, ICastleTransactionsService service)
        {
            this.messageService = messageService;
            this.service = service;
        }

        public void Handle(IDomainMessageCollection messages, SendReminderEMailCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select le.emailaddress from [2am].[dbo].legalentity le ");
            sb.AppendLine("INNER JOIN [2am].[dbo].ADUser ad ON le.LegalEntityKey = ad.LegalEntityKey ");
            sb.AppendFormat("where ad.adusername='{0}'", command.CreatorName);

            DataSet ds = service.ExecuteQueryOnCastleTran(sb.ToString(), Databases.TwoAM, null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string emailAddy = ds.Tables[0].Rows[0][0].ToString();
                StringBuilder sbBody = new StringBuilder();
                sbBody.AppendFormat("This is a notice that an application for loan ({0}) is {1}.", command.ApplicationKey, command.Msg);
                sbBody.AppendLine();

                messageService.SendEmailInternal("HALO@SAHomeloans.com", emailAddy, null, null, "Term Change Notice", sbBody.ToString());
            }
        }
    }
}