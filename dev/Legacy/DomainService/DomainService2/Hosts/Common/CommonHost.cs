using DomainService2.SharedServices.Common;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace DomainService2.Hosts.Common
{
    public class CommonHost : HostBase, ICommon
    {
        public CommonHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public System.DateTime GetnWorkingDaysFromToday(IDomainMessageCollection messages, int nDays)
        {
            var command = new GetnWorkingDaysFromTodayCommand(nDays);
            this.CommandHandler.HandleCommand<GetnWorkingDaysFromTodayCommand>(messages, command);
            return command.Result;
        }

        public string GetCaseName(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetCaseNameCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.CaseNameResult;
        }

        public int GetApplicationKeyFromSourceInstanceID(IDomainMessageCollection messages, long instanceID)
        {
            var command = new GetApplicationKeyFromSourceInstanceIDCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ApplicationKeyResult;
        }

        public void UpdateAssignedUserInIDM(IDomainMessageCollection messages, int applicationKey, bool isFurtherLoan, long instanceID, string mapName)
        {
            var command = new UpdateAssignedUserInIDMCommand(applicationKey, isFurtherLoan, instanceID, mapName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public System.DateTime GetFollowupTime(IDomainMessageCollection messages, int MemoKey)
        {
            var command = new GetFollowupTimeCommand(MemoKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void UpdateParentVars(IDomainMessageCollection messages, long childInstanceID, System.Collections.Generic.Dictionary<string, object> dict)
        {
            var command = new UpdateParentVarsCommand(childInstanceID, dict);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void UpdateOfferStatus(IDomainMessageCollection messages, int applicationKey, int offerStatusKey, int offerInformationTypeKey)
        {
            var command = new UpdateOfferStatusCommand(applicationKey, offerStatusKey, offerInformationTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SetOfferEndDate(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new SetOfferEndDateCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void RecalculateHouseHoldIncomeAndSave(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new RecalculateHouseHoldIncomeAndSaveCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void CreateNewRevision(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new CreateNewRevisionCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void UpdateAccountStatus(IDomainMessageCollection messages, int applicationKey, int accountStatusKey)
        {
            var command = new UpdateAccountStatusCommand(applicationKey, accountStatusKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void CreateAccountForApplication(IDomainMessageCollection messages, int applicationKey, string ADUserName)
        {
            var command = new CreateAccountForApplicationCommand(applicationKey, ADUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool IsValuationInProgress(IDomainMessageCollection messages, long instanceID, int genericKey)
        {
            var command = new IsValuationInProgressCommand(instanceID, genericKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void SendSMSToMainApplicants(IDomainMessageCollection messages, string message, int applicationKey)
        {
            var command = new SendSMSToMainApplicantsCommand(message, applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void PricingForRisk(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new PricingForRiskCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool DoCreditScore(IDomainMessageCollection messages, int applicationKey, int callingContextKey, string ADUserName, bool ignoreWarnings)
        {
            var command = new DoCreditScoreCommand(applicationKey, callingContextKey, ADUserName, ignoreWarnings);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public int GetCreditScoreDecisionKey(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetCreditScoreDecisionKeyCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void ArchiveV3ITCForApplication(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ArchiveV3ITCForApplicationCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendClientSurveyInternalEmail(IDomainMessageCollection messages, int businessEventQuestionnaireKey, int applicationKey, string ADUserName)
        {
            var command = new SendClientSurveyInternalEmailCommand(businessEventQuestionnaireKey, applicationKey, ADUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendClientSurveyExternalEmail(IDomainMessageCollection messages, int businessEventQuestionnaireKey, int applicationKey, string ADUserName)
        {
            var command = new SendClientSurveyExternalEmailCommand(businessEventQuestionnaireKey, applicationKey, ADUserName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool AddDetailType(IDomainMessageCollection messages, int applicationKey, string ADUser, string detailType)
        {
            var command = new AddDetailTypeCommand(applicationKey, ADUser, detailType);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveDetailFromAccount(IDomainMessageCollection messages, int applicationKey, string detailType)
        {
            var command = new RemoveDetailFromAccountCommand(applicationKey, detailType);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void AddReasons(IDomainMessageCollection messages, int genericKey, int reasonDescriptionKey, int reasonTypeKey)
        {
            var command = new AddReasonsCommand(genericKey, reasonDescriptionKey, reasonTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void SendReminderEMail(IDomainMessageCollection messages, string creatorName, int applicationKey, string msg)
        {
            var command = new SendReminderEMailCommand(creatorName, applicationKey, msg);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool PerformEWorkAction(IDomainMessageCollection messages, string eFolderID, string actionToPerform, int genericKey, string assignedUser, string currentStage)
        {
            var command = new PerformEWorkActionCommand(eFolderID, actionToPerform, genericKey, assignedUser, currentStage);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CloneExistsForParent(IDomainMessageCollection messages, long childInstanceId, System.Collections.Generic.List<string> states)
        {
            var command = new CloneExistsForParentCommand(childInstanceId, states);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckUserInMandate(IDomainMessageCollection messages, int applicationKey, string ADuserName, string orgStructureName)
        {
            var command = new CheckUserInMandateCommand(applicationKey, ADuserName, orgStructureName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckIfUserIsPartOfOrgStructure(IDomainMessageCollection messages, SAHL.Common.Globals.OrganisationStructure organisationStructure, string ADUserName)
        {
            var command = new CheckIfUserIsPartOfOrgStructureCommand(organisationStructure, ADUserName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool HasInstancePerformedActivity(IDomainMessageCollection messages, System.Int64 instanceID, string activity)
        {
            var command = new HasInstancePerformedActivityCommand(instanceID, activity);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckPropertyExists(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new CheckPropertyExistsCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckLightStoneValuationRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWanrnings)
        {
            var command = new CheckLightStoneValuationRulesCommand(applicationKey, ignoreWanrnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void DoLightStoneValuationForWorkFlow(IDomainMessageCollection messages, int applicationKey, string adusername)
        {
            var command = new DoLightStoneValuationForWorkFlowCommand(applicationKey, adusername);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public int GetApplicationType(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetApplicationTypeCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ApplicationTypeKeyResult;
        }

        public int GetApplicationStatus(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetApplicationStatusCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ApplicationStatusKeyResult;
        }

        public int GetLatestReasonDescriptionKeyForGenericKey(IDomainMessageCollection messages, int genericKey, int genericKeyTypeKey)
        {
            var command = new GetLatestReasonDescriptionKeyForGenericKeyCommand(genericKey, genericKeyTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string GetLegalEntityLegalName(IDomainMessageCollection messages, int legalEntityKey, SAHL.Common.BusinessModel.Interfaces.LegalNameFormat legalNameFormat)
        {
            var command = new GetLegalEntityLegalNameCommand(legalEntityKey, legalNameFormat);
            this.CommandHandler.HandleCommand(messages, command);
            return command.LegalNameResult;
        }

        public string GetPreviousStateName(IDomainMessageCollection messages, long instanceID)
        {
            var command = new GetPreviousStateNameCommand(instanceID);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void OptOutNonPerformingLoan(IDomainMessageCollection messages, int accountKey)
        {
            var command = new OptOutNonPerformingLoanCommand(accountKey);
            base.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckApplicationMinimumIncomeRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWanrnings)
        {
            var command = new CheckApplicationMinimumIncomeRulesCommand(applicationKey, ignoreWanrnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void CalculateAndSaveApplication(IDomainMessageCollection messages, int applicationKey, bool isBondExceptionAction)
        {
            var command = new CalculateAndSaveApplicationCommand(applicationKey, isBondExceptionAction);
            base.CommandHandler.HandleCommand(messages, command);
        }

        public bool IsRCSAccount(IDomainMessageCollection messages, int AccountKey)
        {
            var command = new IsRCSAccountCommand(AccountKey);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void ProcessApplicationForManuallySelectedEmploymentType(IDomainMessageCollection messages, int applicationKey, bool isBondExceptionAction, int employmentTypeKey)
        {
            var command = new ProcessApplicationForManuallySelectedEmploymentTypeCommand(applicationKey, isBondExceptionAction, employmentTypeKey);
            base.CommandHandler.HandleCommand(messages, command);
        }

        public bool IsComcorpApplication(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new IsComcorpApplicationCommand(applicationKey);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool CheckIsComcorpApplicationRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings)
        {
            var command = new CheckIsComcorpApplicationRuleCommand(applicationKey, ignoreWarnings);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public int GetComcorpVendorOrgStructureByApplication(IDomainMessageCollection messages, int applicationKey)
        {
            var command = new GetComcorpVendorOrgStructureByApplicationCommand(applicationKey);
            base.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void ClearRuleCache(IDomainMessageCollection messages)
        {
            this.CommandHandler.HandleCommand<ClearRuleCacheCommand>(messages, new ClearRuleCacheCommand());
        }

        public void ClearCache(IDomainMessageCollection messages, object data)
        {
            var command = new ClearCacheCommand(data);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckApplicationHasAttribute(IDomainMessageCollection messages, int applicationKey, int applicationAttributeTypeKey)
        {
            CheckApplicationHasAttributeCommand command = new CheckApplicationHasAttributeCommand(applicationKey, applicationAttributeTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void RemoveApplicationAttribute(IDomainMessageCollection messages, int applicationKey, int applicationAttributeTypeKey)
        {
            RemoveApplicationAttributeCommand command = new RemoveApplicationAttributeCommand(applicationKey, applicationAttributeTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ConfirmApplicationAffordabilityAssessments(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, int applicationKey)
        {
            var command = new ConfirmApplicationAffordabilityAssessmentsCommand(applicationKey);
            this.CommandHandler.HandleCommand(messages, command);
        }
    }
}