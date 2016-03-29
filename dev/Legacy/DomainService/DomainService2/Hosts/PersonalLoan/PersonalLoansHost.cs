using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using X2DomainService.Interface.PersonalLoan;

namespace DomainService2.Hosts.PersonalLoan
{
    public class PersonalLoanHost : HostBase, IPersonalLoan
    {
        public PersonalLoanHost(ICommandHandler commandHandler)
            : base(commandHandler)
        { }

        public bool CheckCreditSubmissionRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCreditSubmissionRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCreditSubmissionClientRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCreditSubmissionClientRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckSendOfferRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckSendOfferRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string GetInstanceSubjectForPersonalLoan(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetInstanceSubjectForPersonalLoanCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void UpdateOfferInformationType(IDomainMessageCollection messages, int applicationKey, SAHL.Common.Globals.OfferInformationTypes informationType)
        {
            var command = new UpdateOfferInformationTypeCommand(applicationKey, (int)informationType);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendSMSToApplicantUponDisbursement(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new SendSMSToApplicantUponDisbursementCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckDisbursementCutOffTimeRule(IDomainMessageCollection messages, bool ignoreWarnings)
        {
            var command = new CheckDisbursementCutOffTimeRuleCommand(ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void DisburseFunds(IDomainMessageCollection messages, int applicationKey, string userID)
        {
            var command = new DisburseFundsCommand(applicationKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public string GetADUserNameByWorkflowRoleType(IDomainMessageCollection messages, int applicationKey, int workflowRoleTypeKey)
        {
            GetADUserNameByWorkflowRoleTypeCommand command = new GetADUserNameByWorkflowRoleTypeCommand(applicationKey, workflowRoleTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public void EmailCorrespondenceReportToApplicationMailingAddress(IDomainMessageCollection messages, int genericKey, string adusername, string reportName, CorrespondenceTemplates correspondenceTemplate)
        {
            EmailCorrespondenceReportToApplicationMailingAddressCommand command = new EmailCorrespondenceReportToApplicationMailingAddressCommand(genericKey, adusername, reportName, correspondenceTemplate);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckUnderDebtCounsellingRule(IDomainMessageCollection messages, bool ignoreWarnings, int applicaitonKey)
        {
            var command = new CheckUnderDebtCounsellingRuleCommand(applicaitonKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCanEmailPersonalLoanApplicationRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCanEmailPersonalLoanApplicationRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void CreateAndOpenPersonalLoan(IDomainMessageCollection messages, int applicationKey, string userID)
        {
            var command = new CreateAndOpenPersonalLoanCommand(applicationKey, userID);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ReturnDisbursedPersonalLoanToApplication(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ReturnDisbursedPersonalLoanToApplicationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckAlteredApprovalStageTransitionRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckAlteredApprovalStageTransitionRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void ActivatePendingDomiciliumAddress(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ActivatePendingDomiciliumAddressCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

	public bool CheckCededAmountCoversApplicationAmountRule(IDomainMessageCollection messages, int applicationKey, double sumInsured, bool ignoreWarnings)
        {
            var command = new CheckCededAmountCoversApplicationAmountRuleCommand(applicationKey, sumInsured, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckExternalPolicyIsCededRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckExternalPolicyIsCededRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ApplicationHasSAHLLifeApplied(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ApplicationHasSAHLLifeAppliedCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}