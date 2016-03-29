using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class SendNotificationsCancellatonRegistered_EXTCommandHandler : IHandlesDomainServiceCommand<SendNotificationsCancellatonRegistered_EXTCommand>
    {
        private IDebtCounsellingRepository debtCounsellingRepository;
        private ICommonRepository commonRepository;
        private IMessageService messageService;

        public SendNotificationsCancellatonRegistered_EXTCommandHandler(IDebtCounsellingRepository debtCounsellingRepository,
                                                                        ICommonRepository commonRepository, IMessageService messageService)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
            this.commonRepository = commonRepository;
            this.messageService = messageService;
        }

        public void Handle(IDomainMessageCollection messages, SendNotificationsCancellatonRegistered_EXTCommand command)
        {
            IDebtCounselling debtCounsellingCase = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
            if (debtCounsellingCase.DebtCounsellor == null)
            {
                messages.Add(new Error("Debt Counsellor for Case does not exist", "Debt Counsellor for Case does not exist"));
            }
            else
            {
                string debtCounsellorEmailAddress = debtCounsellingCase.DebtCounsellor.EmailAddress;
                /*
                 * If there is a Recoveries Proposal Received state transition against the case,
                 * then email the Debt Counselor and notify them that the mortgage bond has been cancelled and they need to continue payments to the Recoveries Proposal.
                 * An eWorks case must be created for Recoveries process and the Recoveries Proposal Data to be inserted as a note on eWorks for their information.
                 */
                ICorrespondenceTemplate correspondenceTemplate;
                if (command.RecoveriesProposalReceivedStageTransitionExists)
                {
                    correspondenceTemplate = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledContinuePaying);
                }
                /*
                 * If there is no Recoveries Proposal Received state transition against the case
                 * then email the Debt Counselor and notify them that the mortgage bond has been cancelled and
                 * they do not need to make any further payments. No eworks case is created for
                 */
                else
                {
                    correspondenceTemplate = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.MortgageLoanCancelledDontContinuePaying);
                }

                bool notificationSendSucceeded = messageService.SendEmailExternal(command.DebtCounsellingKey, "no-reply@sahomeloans.com", debtCounsellorEmailAddress, String.Empty, String.Empty, String.Format(correspondenceTemplate.Subject, debtCounsellingCase.Account.Key.ToString()), String.Format(correspondenceTemplate.Template, debtCounsellingCase.Account.Key.ToString()), "", "", "");
                if (!notificationSendSucceeded)
                {
                    messages.Add(new Error("SendNotificationsCancellatonRegistered_EXT: Notification to Debt Counsellor could not be sent", "SendNotificationsCancellatonRegistered_EXT: Notification to Debt Counsellor could not be sent"));
                }
                else
                {
                    debtCounsellingRepository.SendNotification(debtCounsellingCase);
                }
            }
        }
    }
}