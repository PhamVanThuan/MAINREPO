using DomainService2.Workflow.Origination.ApplicationCapture;
using SAHL.Common.Logging;
using X2DomainService.Interface.Origination;

namespace DomainService2.Hosts.Origination.ApplicationCapture
{
    public class ApplicationCaptureHost : HostBase, IApplicationCapture
    {
        public ApplicationCaptureHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool IsEstateAgentInRole(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, string creatorAdUserName, params string[] roles)
        {
            var command = new IsEstateAgentInRoleCommand(creatorAdUserName, roles);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsEstateAgent(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, string creatorAdUserName)
        {
            var command = new IsEstateAgentCommand(creatorAdUserName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SubmitApplicationToApplicationManagement(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, long instanceID, int applicationKey)
        {
            var command = new SubmitApplicationToApplicationManagementCommand(instanceID, applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendReminderEMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, long instanceID)
        {
            var command = new SendReminderEmailCommand(applicationKey, instanceID);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void CreateCommissionableConsultantRole(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string adUserName)
        {
            var command = new CreateCommissionableConsultantRoleCommand(applicationKey, adUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool DemoteMainApplicantToLead(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new DemoteMainApplicantToLeadCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCreditSubmissionPrimaryITCRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCreditSubmissionPrimaryITCRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckCreditSubmissionSecondaryITCRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckCreditSubmissionSecondaryITCRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckBranchSubmitApplicationRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckBranchSubmitApplicationRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckManagerSubmitApplicationRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckManagerSubmitApplicationRulesCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool PromoteLeadToMainApplicant(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new PromoteLeadToMainApplicantCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CreateInternetLead(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string leadData, bool ignoreWarnings)
        {
            var command = new CreateInternetLeadCommand(applicationKey, leadData);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CreateInternetApplication(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string applicationData, bool ignoreWarnings)
        {
            var command = new CreateInternetApplicationCommand(applicationKey, applicationData);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }


		public void SendNewClientConsultantDetailsSMS(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
		{
			var command = new SendNewClientConsultantDetailsSMSCommand(applicationKey);
			this.CommandHandler.HandleCommand(messages, command);
		}
	}
}