using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;
using System;

namespace X2DomainService.Interface.Origination
{
    public interface IApplicationManagement : IX2WorkflowService
    {
        void SendNTUFinalResubMail(IDomainMessageCollection messages, int applicationKey); //SendNTUFinalResubMail

        void SendEmailToConsultantForQuery(IDomainMessageCollection messages, int genericKey, long instanceID, int reasonGroupTypeKey);

        bool SaveApplication(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckQACompleteRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckCreditOverrideRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings); //OverRideCheck

        bool CheckResubOverRideRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings); //ResubOverRideCheck

        bool CheckApplicationDebitOrderCollectionRule(IDomainMessageCollection messages, int applicationKey, bool ignoreMessages); //LOAAccepted

        bool ArchiveChildInstances(IDomainMessageCollection messages, Int64 instanceID, string ADUser);

        void RemoveRegistrationProcessDetailTypes(IDomainMessageCollection messages, int applicationKey);

        void SetAccountStatusToApplicationPriorToInstructAttorney(IDomainMessageCollection messages, int applicationKey, string adUserName);

        void RemoveAccountFromRegMail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey);

        void ReturnDisbursedLoanToRegistration(IDomainMessageCollection messages, int applicationKey);

        void ReturnNonDisbursedLoanToProspect(IDomainMessageCollection messages, int applicationKey);

        void NTUCase(IDomainMessageCollection messages, int genericKey);

        bool HasApplicationRole(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, int applicationRoleTypeKey);

        void ActivateNTUFromWatchdogTime(IDomainMessageCollection messages, long instanceID);

        bool CheckInstructAttorneyRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings); //InstructAttorney

        bool CheckADUserInSameBranchRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings, string adUserName); //CheckADUserInSameBranch

        void RemoveDetailFromApplicationAfterNTUFinalised(IDomainMessageCollection messages, int applicationKey);

        bool CheckValuationRequiredRules(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        void SendEmailToConsultantForValuationDone(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey);

        bool CheckApplicationHas30YearTermRule(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, bool ignoreWarning);

        void SendAlphaHousingSurveyEmail(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string ADUserName, out bool alphaHousingEmailSent);

        bool HelpDeskNTU(IDomainMessageCollection messages, int applicationKey);

        bool Check30YearTermApplicationRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        void Revert30YearTermApplicationToPreviousTerm(IDomainMessageCollection messages, int applicationKey);

        #region EWork Stuff

        void AddDetailTypeInstructionSent(IDomainMessageCollection messages, int applicationKey);

        bool CreateEWorkPipelineCase(IDomainMessageCollection messages, int applicationKey, out string efolderID); //CreateEWorkCase

        bool CheckEWorkAtCorrectStateRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings); //CheckEWorkAtCorrectState

        void CheckIfReinstateAllowedByUser(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey, string previousState, bool ignoreWarning, string adUserName);

        #endregion EWork Stuff
    }
}