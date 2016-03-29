using DomainService2.Workflow.Origination.Credit;
using System.Collections.Generic;
using X2DomainService.Interface.Origination;

namespace DomainService2.Hosts.Origination.Credit
{
    public class CreditHost : HostBase, ICredit
    {
        public CreditHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public void PerformCreditMandateCheck(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, long instanceID, List<string> loadBalanceStates, bool loadBalanceIncludeStates, bool loadBalance1stPass, bool loadBalance2ndPass)
        {
            PerformCreditMandateCheckCommand command = new PerformCreditMandateCheckCommand(applicationKey, instanceID, loadBalanceStates, loadBalanceIncludeStates, loadBalance1stPass, loadBalance2ndPass);
            this.CommandHandler.HandleCommand<PerformCreditMandateCheckCommand>(messages, command);
        }

        public void CreditResub(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            CreditResubCommand command = new CreditResubCommand(applicationKey);
            this.CommandHandler.HandleCommand<CreditResubCommand>(messages, command);
        }

        public bool CheckCreditApprovalRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var checkCreditApprovalRulesCommand = new CheckCreditApprovalRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, checkCreditApprovalRulesCommand);
            return checkCreditApprovalRulesCommand.Result;
        }

        public bool DoesNotMeetCreditSignatureRequirements(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, long instanceID)
        {
            DoesNotMeetCreditSignatureRequirementsCommand command = new DoesNotMeetCreditSignatureRequirementsCommand(applicationKey, instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void UpdateConditions(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            UpdateConditionsCommand command = new UpdateConditionsCommand(applicationKey);
            this.CommandHandler.HandleCommand<UpdateConditionsCommand>(messages, command);
        }

        public void SendResubMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            SendResubMailCommand command = new SendResubMailCommand(applicationKey);
            this.CommandHandler.HandleCommand<SendResubMailCommand>(messages, command);
        }

        public bool IsReviewRequired(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, string activityName)
        {
            IsReviewRequiredCommand command = new IsReviewRequiredCommand(instanceID, activityName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsCreditSecondPass(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID)
        {
            var command = new IsCreditSecondPassCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsValuationApprovalRequired(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID)
        {
            var command = new IsValuationApprovalRequiredCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendCreditDecisionMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, string action, int offerKey)
        {
            var command = new SendCreditDecisionMailCommand(instanceID, action, offerKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckEmploymentTypeConfirmedRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, bool ignoreWarnings)
        {
            var checkEmploymentTypeConfirmedRuleCommand = new CheckEmploymentTypeConfirmedRuleCommand(instanceID, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, checkEmploymentTypeConfirmedRuleCommand);
            return checkEmploymentTypeConfirmedRuleCommand.Result;
        }

        public bool CheckApplicationIsNewBusinessRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var checkApplicationIsNewBusinessRuleCommand = new CheckApplicationIsNewBusinessRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, checkApplicationIsNewBusinessRuleCommand);
            return checkApplicationIsNewBusinessRuleCommand.Result;
        }

        public void DisqualifyApplicationForGEPF(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            DisqualifyApplicationForGEPFCommand command = new DisqualifyApplicationForGEPFCommand(applicationKey);
            this.CommandHandler.HandleCommand<DisqualifyApplicationForGEPFCommand>(messages, command);
        }
    }
}