using DomainService2.Workflow.Origination.ApplicationManagement;
using X2DomainService.Interface.Origination;

namespace DomainService2.Hosts.Origination.ApplicationManagement
{
    public class ApplicationManagementHost : HostBase, IApplicationManagement
    {
        public ApplicationManagementHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public void SendNTUFinalResubMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new SendNTUFinalResubMailCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendEmailToConsultantForQuery(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int genericKey, long instanceID, int reasonGroupTypeKey)
        {
            var command = new SendEmailToConsultantForQueryCommand(genericKey, instanceID, reasonGroupTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool SaveApplication(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new SaveApplicationCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckQACompleteRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckQACompleteRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCreditOverrideRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCreditOverrideRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckResubOverRideRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckResubOverRideRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckApplicationDebitOrderCollectionRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreMessages)
        {
            var command = new CheckApplicationDebitOrderCollectionRuleCommand(applicationKey, ignoreMessages);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ArchiveChildInstances(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, string adUserName)
        {
            var command = new ArchiveChildInstancesCommand(instanceID, adUserName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveAccountFromRegMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new RemoveAccountFromRegMailCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ReturnDisbursedLoanToRegistration(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ReturnDisbursedLoanToRegistrationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ReturnNonDisbursedLoanToProspect(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ReturnNonDisbursedLoanToProspectCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void NTUCase(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new NTUCaseCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool HasApplicationRole(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, int applicationRoleTypeKey)
        {
            var command = new HasApplicationRoleCommand(applicationKey, applicationRoleTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveDetailFromApplicationAfterNTUFinalised(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new RemoveDetailFromApplicationAfterNTUFinalisedCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void AddDetailTypeInstructionSent(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new AddDetailTypeInstructionSentCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CreateEWorkPipelineCase(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, out string efolderID)
        {
            var command = new CreateEWorkPipelineCaseCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            efolderID = command.EFolderID;
            return command.Result;
        }

        public bool CheckValuationRequiredRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckValuationRequiredRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveRegistrationProcessDetailTypes(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new RemoveRegistrationProcessDetailTypesCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SetAccountStatusToApplicationPriorToInstructAttorney(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string adUserName)
        {
            var command = new SetAccountStatusToApplicationPriorToInstructAttorneyCommand(applicationKey, adUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ActivateNTUFromWatchdogTime(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID)
        {
            var command = new ActivateNTUFromWatchdogTimeCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckInstructAttorneyRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckInstructAttorneyRulesCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckEWorkAtCorrectStateRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckEWorkAtCorrectStateRuleCommand(applicationKey, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckADUserInSameBranchRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings, string adUserName)
        {
            var command = new CheckADUserInSameBranchRulesCommand(applicationKey, adUserName, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendEmailToConsultantForValuationDone(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new SendEmailToConsultantForValuationDoneCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void CheckIfReinstateAllowedByUser(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string previousState, bool ignoreWarning, string adUserName)
        {
            var command = new CheckIfReinstateAllowedByUserCommand(applicationKey, previousState, ignoreWarning, adUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckApplicationHas30YearTermRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarning)
        {
            var command = new CheckApplicationHas30YearTermRuleCommand(applicationKey, ignoreWarning);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendAlphaHousingSurveyEmail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string ADUserName, out bool AlphaHousingEmailSent)
        {
            var command = new SendAlphaHousingSurveyEmailCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            AlphaHousingEmailSent = command.AlphaHousingEmailSent;
        }

        public bool HelpDeskNTU(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new HelpDeskNTUCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool Check30YearTermApplicationRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreMessages)
        {
            var command = new Check30YearTermApplicationRuleCommand(applicationKey, ignoreMessages);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void Revert30YearTermApplicationToPreviousTerm(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            Revert30YearTermApplicationToPreviousTermCommand command = new Revert30YearTermApplicationToPreviousTermCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }
    }
}