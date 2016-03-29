using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;
using System;
using System.Collections.Generic;

namespace X2DomainService.Interface.Common
{
    public interface ICommon : IX2WorkflowService
    {
        DateTime GetnWorkingDaysFromToday(IDomainMessageCollection messages, int nDays);

        string GetCaseName(IDomainMessageCollection messages, int applicationKey);

        string GetLegalEntityLegalName(IDomainMessageCollection messages, int legalEntityKey, LegalNameFormat legalNameFormat);

        int GetApplicationKeyFromSourceInstanceID(IDomainMessageCollection messages, Int64 instanceID);

        int GetApplicationType(IDomainMessageCollection messages, int applicationKey);

        int GetApplicationStatus(IDomainMessageCollection messages, int applicationKey);

        void UpdateAssignedUserInIDM(IDomainMessageCollection messages, int applicationKey, bool isFurtherLoan, Int64 instanceID, string mapName);

        DateTime GetFollowupTime(IDomainMessageCollection messages, int MemoKey);

        void UpdateParentVars(IDomainMessageCollection messages, Int64 childInstanceID, Dictionary<string, object> dict);

        void UpdateOfferStatus(IDomainMessageCollection messages, int applicationKey, int offerStatusKey, int offerInformationTypeKey);

        void SetOfferEndDate(IDomainMessageCollection messages, int applicationKey);

        void RecalculateHouseHoldIncomeAndSave(IDomainMessageCollection messages, int applicationKey);

        void CreateNewRevision(IDomainMessageCollection messages, int applicationKey);

        void UpdateAccountStatus(IDomainMessageCollection messages, int applicationKey, int accountStatusKey);

        void CreateAccountForApplication(IDomainMessageCollection messages, int applicationKey, string ADUserName);

        bool IsValuationInProgress(IDomainMessageCollection messages, Int64 instanceID, int genericKey);

        void SendSMSToMainApplicants(IDomainMessageCollection messages, string message, int applicationKey);

        void PricingForRisk(IDomainMessageCollection messages, int applicationKey);

        //bool DoCreditScore(IDomainMessageCollection messages, int applicationKey, int callingContextKey, string ADUserName, bool ignoreWarnings);

        int GetCreditScoreDecisionKey(IDomainMessageCollection messages, int applicationKey);

        void ArchiveV3ITCForApplication(IDomainMessageCollection messages, int applicationKey);

        void SendClientSurveyInternalEmail(IDomainMessageCollection messages, int businessEventQuestionnaireKey, int applicationKey, string ADUserName);

        void SendClientSurveyExternalEmail(IDomainMessageCollection messages, int businessEventQuestionnaireKey, int applicationKey, string ADUserName);

        bool AddDetailType(IDomainMessageCollection messages, int applicationKey, string ADUser, string detailType);

        void RemoveDetailFromAccount(IDomainMessageCollection messages, int applicationKey, string detailType);

        void AddReasons(IDomainMessageCollection messages, int genericKey, int reasonDescriptionKey, int reasonTypeKey);

        void SendReminderEMail(IDomainMessageCollection messages, string creatorName, int applicationKey, string msg);

        bool PerformEWorkAction(IDomainMessageCollection messages, string eFolderID, string actionToPerform, int genericKey, string assignedUser, string currentStage);

        bool CloneExistsForParent(IDomainMessageCollection messages, long childInstanceId, List<string> states);

        bool CheckUserInMandate(IDomainMessageCollection messages, int applicationKey, string ADuserName, string orgStructureName);

        bool CheckIfUserIsPartOfOrgStructure(IDomainMessageCollection messages, SAHL.Common.Globals.OrganisationStructure organisationStructure, string ADUserName);

        bool CheckPropertyExists(IDomainMessageCollection messages, int applicationKey);

        bool CheckLightStoneValuationRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWanrnings);

        void DoLightStoneValuationForWorkFlow(IDomainMessageCollection messages, int applicationKey, string adusername);

        bool HasInstancePerformedActivity(IDomainMessageCollection messages, System.Int64 instanceID, string activity);

        int GetLatestReasonDescriptionKeyForGenericKey(IDomainMessageCollection messages, int genericKey, int genericKeyTypeKey);

        string GetPreviousStateName(IDomainMessageCollection messages, long instanceID);

        void OptOutNonPerformingLoan(IDomainMessageCollection messages, int accountKey);

        bool CheckApplicationMinimumIncomeRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWanrnings);

        void CalculateAndSaveApplication(IDomainMessageCollection messages, int applicationKey, bool isBondExceptionAction);

        bool IsRCSAccount(IDomainMessageCollection messages, int AccountKey);

        void ProcessApplicationForManuallySelectedEmploymentType(IDomainMessageCollection messages, int applicationKey, bool isBondExceptionAction, int employmentTypeKey);

        void ClearCache(IDomainMessageCollection messages, object data);

        void ClearRuleCache(IDomainMessageCollection messages);

        bool IsComcorpApplication(IDomainMessageCollection messages, int applicationKey);

        bool CheckIsComcorpApplicationRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        int GetComcorpVendorOrgStructureByApplication(IDomainMessageCollection messages, int applicationKey);

        bool CheckApplicationHasAttribute(IDomainMessageCollection messages, int applicationKey, int applicationAttributeTypeKey);

        void RemoveApplicationAttribute(IDomainMessageCollection messages, int applicationKey, int applicationAttributeTypeKey);

        void ConfirmApplicationAffordabilityAssessments(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey);
    }
}