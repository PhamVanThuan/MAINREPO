using Automation.DataAccess;
using Automation.DataModels;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IX2WorkflowService
    {
        void InsertActiveExternalActivity(string workflowName, string externalActivity, Int64 instanceID, int genericKey, string activityName = "", string activityXMLData = null);

        void WaitForX2WorkflowHistoryActivity(string activityName, Int64 instanceID, int count);

        void WaitForX2WorkflowHistoryActivity(int instanceID, int count, string dateFilter, params string[] activityName);

        void WaitForAppManCaseCreate(int offerKey);

        Int64 GetAppManInstanceIDByState(string workflowState, int offerKey, bool includeClones = false);

        void PipeLineReinstateNTU(int offerKey);

        void PipeLineNTU(int offerKey);

        void PipelineUpForFees(int offerKey);

        void PipelineComplete(int offerKey);

        void PipeLineHeldOver(int offerKey);

        int GetOfferKeyAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions);

        int GetOfferKeyAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions, int worklistADUserStatus);

        int GetOfferKeyAtStateByTypeAndLoanAmount(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions, int LoanAmount);

        QueryResults GetOfferKeysAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions);

        QueryResults GetOfferKeysAtStateByType(string state, string workflow, string exclusions, OfferTypeEnum offerType, double maxLTV, double minLTV,
            int numLE, OccupancyTypeEnum occupancyType = OccupancyTypeEnum.OwnerOccupied, int employmentType = 0, int category = -1);

        QueryResults GetApplicationsByStateAndAppType(string state, string workflow, string exclusions, string offerTypeKey, double maxLTV, double minLTV,
            int numLE, OccupancyTypeEnum occupancyType = OccupancyTypeEnum.OwnerOccupied, int employmentType = 0, int category = -1, double maxIncome = 500000, double minIncome = 0);

        List<int> GetOffersAtState(string state, string workflow, string exclusions);

        void WaitForCreditCaseCreate(Int64 sourceInstanceID, int offerKey, string expectedCreditState);

        void WaitForX2State(int generickey, string workflowName, string workflowstate, int noTries = 12);

        IEnumerable<Automation.DataModels.X2Instance> GetManyWorkflowInstances(string workflowName, params string[] workflowStateNames);

        int GetWorkflowCaseWithoutBusinessEvent(string workflowState, string workflow, OfferTypeEnum offerType, StageDefinitionStageDefinitionGroupEnum businessEvent);

        QueryResults GetX2DataByWorkflowAndState(string sWorkflowName, string sStateName, int nowRows = 0);

        int GetAppCapInstanceIDByState(int offerKey, string workflowState);

        int GetCreditInstanceIDByState(string workflowState, int offerKey, bool includeClones = false);

        int GetReadvancePaymentsInstanceIDByState(int offerKey, string workflowState);

        int GetInstanceIDByDebtCounsellingKey(int debtCounsellingKey);

        QueryResults GetScheduledActivityTime(string instanceName, string activityName);

        QueryResults GetOfferKeysAtStateByAdUser(string stateName, string adUserName);

        int GetStateIDByName(string stateName, string workflowName);

        int GetMaxWorkflowID(string workflowName);

        QueryResults GetActiveWorkflowRoleAssignmentByInstance(Int64 instanceID);

        QueryResults GetWorkflowInstanceForStateAndADUser(string map, int genericKey, string state, string aduserName);

        bool DoesApplicationExistAtAppManageState(int applicationKey, string state);

        int GetCountofCasesForUser(string stateName, string workflowName, string userName);

        bool WaitForX2ScheduledActivity(string activityName, Int64 instanceID);

        IEnumerable<Automation.DataModels.X2ScheduledActivity> GetScheduledActivities(Int64 instanceid);

        IEnumerable<Automation.DataModels.X2Workflow> GetLatestWorkflows();

        IEnumerable<Automation.DataModels.X2Activity> GetManyActivitiesByManyLatestWorkflows();

        List<Automation.DataModels.X2Activity> GetManyActivitiesBySingleLatestWorkflow(string workflowName);

        void WaitForValuationsCaseCreate(Int64 sourceInstanceID, int offerKey, string expectedValuationState);

        bool CloneExists(int offerKey, string[] states, string workflow);

        QueryResults GetInstanceCloneByState(string instanceName, string state, string workflow);

        QueryResults GetTop1InstanceCloneByState(string instanceName, string state, string workflow);

        IEnumerable<Automation.DataModels.X2WorkflowList> GetWorklistDetails(int offerKey, string state, string workflow);

        QueryResults GetInstanceDetails(Int64 instanceID);

        QueryResults GetAppCapInstanceDetails(int offerKey);

        QueryResults GetDisabilityClaimInstanceDetails(int disabilityClaimKey);

        QueryResults GetAppManInstanceDetails(int offerKey, bool includeClones = false);

        QueryResults GetValuationsInstanceDetails(int offerKey);

        QueryResults GetReadvancePaymentsInstanceDetails(int offerKey);

        QueryResults GetCreditInstanceDetails(int offerKey);

        DateTime CalculateFutureDateInBusinessDays(int Days, DateTime? timerBaseValue = null);

        QueryResults GetScheduledActivityTimeForCloneInstance(int offerKey, string activityName);

        QueryResults GetScheduledActivityTimeForInstance(int OfferKey, string ActivityName);

        QueryResults GetWorkflowHistoryActivityCount(Int64 instanceID, string activity, string dateFilter = "");

        QueryResults GetWorkflowHistoryActivitiesCount(int instanceID, string dateFilter = "", params string[] activity);

        QueryResults GetDeleteDebitOrderInstanceDetails(int accountKey);

        QueryResults GetDebtCounsellingInstanceDetails(int debtCounsellingKey = 0, Int64 instanceID = 0);

        IEnumerable<LifeLead> GetLifeInstanceDetails(int offerKey, int accountkey);

        QueryResults GetLoanAdjustmentInstance(int accountKey, int loanAdjustmentType, string state);

        bool IsActivityTimedOut(int accountKey);

        QueryResults GetlatestX2DataApplicationManagementRow(int applicationKey);

        QueryResults GetWorklistDetails(Int64 instanceid);

        QueryResults GetOpenOffersForPreSubmission(int recordCount);

        QueryResults GetOpenApplicationManagementOffers(int records);

        QueryResults GetWorkflowInstanceForStateADUserAndOfferType(string stateName, string aduserName, params int[] offerTypeKey);

        int GetAppManOffers_FilterByValuationsAndWorkflowHistory(string workflowHistoryState, int stateFilterType, int valuationFilterType,
                    int requiresValuationFlag, params int[] offerTypeKeys);

        int GetAppManOfferKey_FilterByClone(string state, string adUser, int includeExclude, string filterByCloneState);

        int GetAppManOfferKey_FilterByClone(string state, string adUser, int includeExclude, string filterByCloneState, int requireValuationFlag,
            params int[] offerTypeKeys);

        int GetOffersWithoutLightstonePropertyID(string workflow, string state);

        bool OfferExistsAtState(int offerKey, string workflowState);

        Int64 GetPersonalLoanInstanceId(int applicationKey);

        bool CheckUserRoleSecuritySetup(string workflowName, string userRole, string stateName, string activityName);

        int GetAppManInstanceIDByOfferKey(int offerKey);

        string GetPreviousStateForInstancePriorToActivity(Int64 instanceID, string workflowActivity);

        List<Automation.DataModels.Offer> GetXNumberOfOffersAtQA(int x, bool isFL);

        List<Automation.DataModels.Offer> GetXNumberOfCaseForAppInOrder(int x);

        List<Automation.DataModels.Offer> GetAppManOfferWithoutValuation();

        void SetIsValuationRequiredIndicator(bool isRequired, Int64 instanceId);

        bool IsValuationRequiredIndicator(Int64 instanceId);

        bool IsValuationsAtArchiveState(int applicationKey);

        string GetWorkflowState(int applicationKey);

        IEnumerable<Automation.DataModels.Offer> GetOffersAtStateWithAmendedValuation(string state, string workflow);

        IEnumerable<Automation.DataModels.Offer> GetOffersAtStateWithoutAmendedValuation(string state, string workflow);

        Automation.DataModels.Offer GetPersonalLoanOfferAtState(string stateName, bool hasSAHLLife = false, bool hasExternalLife = false);

        Automation.DataModels.Offer GetOfferAtCreditWithUserConfirmedApplicationEmployment(bool isUserConfirmedApplicationEmployment = true);

        void UpdateAlphaHousingSurveyEmailSent(int offerKey, bool alphaHousingSurveyEmailSent);

        Automation.DataModels.Offer GetOfferWithOfferAttributeAtState(WorkflowEnum workflow, string applicationManagementStateName, OfferAttributeTypeEnum offerAttributeTypeKey, bool offerAttributeExists);

        IEnumerable<Automation.DataModels.X2Request> GetLastestX2Requests();

        Automation.DataModels.DisabilityClaim GetDisabilityClaimAtState(string state);
    }
}