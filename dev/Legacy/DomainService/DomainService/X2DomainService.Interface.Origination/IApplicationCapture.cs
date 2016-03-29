using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Origination
{
    public interface IApplicationCapture : IX2WorkflowService
    {
        bool IsEstateAgentInRole(IDomainMessageCollection messages, string creatorAdUserName, params string[] roles);

        bool IsEstateAgent(IDomainMessageCollection messages, string creatorAdUserName);

        void SubmitApplicationToApplicationManagement(IDomainMessageCollection messages, long instanceID, int applicationKey);

        void SendReminderEMail(IDomainMessageCollection messages, int applicationKey, Int64 instanceID);

        void CreateCommissionableConsultantRole(IDomainMessageCollection messages, int applicationKey, string ADUserName);

        bool CheckBranchSubmitApplicationRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool DemoteMainApplicantToLead(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckManagerSubmitApplicationRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool PromoteLeadToMainApplicant(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckCreditSubmissionPrimaryITCRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckCreditSubmissionSecondaryITCRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CreateInternetLead(IDomainMessageCollection messages, int applicationKey, string leadData, bool ignoreWarnings);

        bool CreateInternetApplication(IDomainMessageCollection messages, int applicationKey, string applicationData, bool ignoreWarnings);

		void SendNewClientConsultantDetailsSMS(IDomainMessageCollection messages, int applicationKey);
    }
}