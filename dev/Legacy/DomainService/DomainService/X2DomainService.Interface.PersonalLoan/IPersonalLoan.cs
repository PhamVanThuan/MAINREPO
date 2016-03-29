using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.X2.Common;

namespace X2DomainService.Interface.PersonalLoan
{
    public interface IPersonalLoan : IX2WorkflowService
    {
        bool CheckCreditSubmissionRules(IDomainMessageCollection message, int applicationKey, bool ignoreWarnings);

        bool CheckCreditSubmissionClientRules(IDomainMessageCollection message, int applicationKey, bool ignoreWarnings);

        bool CheckSendOfferRules(IDomainMessageCollection message, int applicationKey, bool ignoreWarnings);

        string GetInstanceSubjectForPersonalLoan(IDomainMessageCollection messages, int applicationKey);

        void UpdateOfferInformationType(IDomainMessageCollection messages, int applicationKey, OfferInformationTypes informationType);

        void SendSMSToApplicantUponDisbursement(IDomainMessageCollection messages, int applicationKey);

        bool CheckDisbursementCutOffTimeRule(IDomainMessageCollection messages, bool ignoreWarnings);

        void DisburseFunds(IDomainMessageCollection messages, int applicationKey, string userID);

        string GetADUserNameByWorkflowRoleType(IDomainMessageCollection messages, int applicationKey, int workflowRoleTypeKey);

        void EmailCorrespondenceReportToApplicationMailingAddress(IDomainMessageCollection messages, int genericKey, string adusername, string reportName, CorrespondenceTemplates correspondenceTemplate);

        bool CheckUnderDebtCounsellingRule(IDomainMessageCollection messages, bool ignoreWarnings, int applicaitonKey);

        bool CheckCanEmailPersonalLoanApplicationRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        void CreateAndOpenPersonalLoan(IDomainMessageCollection messages, int applicationKey, string userID);

        void ReturnDisbursedPersonalLoanToApplication(IDomainMessageCollection messages, int applicationKey);

        bool CheckAlteredApprovalStageTransitionRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        void ActivatePendingDomiciliumAddress(IDomainMessageCollection messages, int applicationKey);

	bool CheckCededAmountCoversApplicationAmountRule(IDomainMessageCollection messages, int applicationKey, double sumInsured, bool ignoreWarnings);

	bool CheckExternalPolicyIsCededRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool ApplicationHasSAHLLifeApplied(IDomainMessageCollection messages, int applicationKey);
    }
}