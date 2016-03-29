using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.Credit
{
    public class SendCreditDecisionMailCommandHandler : IHandlesDomainServiceCommand<SendCreditDecisionMailCommand>
    {
        IMessageService messageService;
        ICommonRepository commonRepository;
        IApplicationRepository applicationRepository;

        public SendCreditDecisionMailCommandHandler(IMessageService messageService, ICommonRepository commonRepository, IApplicationRepository applicationRepository)
        {
            this.messageService = messageService;
            this.commonRepository = commonRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendCreditDecisionMailCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.OfferKey);
            if (application.ApplicationType.Key == (int)OfferTypes.SwitchLoan || application.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan || application.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)
            {

                var users = application.ApplicationRoles.Where(
                    x => x.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        ( x.ApplicationRoleType.Key == (int)OfferRoleTypes.BranchConsultantD
                        || x.ApplicationRoleType.Key == (int)OfferRoleTypes.BranchAdminD
                        )
                    )
                    .Select(x => x.LegalEntity);

                ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.CreditDecisionInternal);

                string cc = string.Empty;
                string bcc = string.Empty;
                string clients = application.GetLegalName(LegalNameFormat.Full);
                string subject = string.Format(template.Subject, command.Action, command.OfferKey);
                string body = string.Format(template.Template, command.Action, command.OfferKey, clients);

                foreach (ILegalEntity le in users)
                {
                    if (!String.IsNullOrEmpty(le.EmailAddress))
                        messageService.SendEmailInternal(SAHL.Common.ApplicationManagement.EmailAddresses.FromHalo, le.EmailAddress, cc, bcc, subject, body);
                }
            }
        }
    }
}
