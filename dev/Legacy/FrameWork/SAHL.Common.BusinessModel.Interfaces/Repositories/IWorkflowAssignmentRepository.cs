using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IWorkflowAssignmentRepository
    {
        string GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(int OfferRoleTypeKey, int ApplicationKey, long InstanceID);

        string ReassignToMostSenPersonWhoWorkedOnThisCaseInCredit(long InstanceID, long SourceInstanceID, int OfferKey, string State);

        bool CheckUserInMandate(int applicationKey, string aduserName, string orgStructureName);

        void GetFirstAssignedCreditUser(long SourceInstanceID, out string adUserName, out int offerRoleTypeKey, out int orgStructureKey);

        string AssignCaseThatWasPreviouslyInDisputeIndicated(int offerKey, long instanceID);

        bool AssignCreateorAsDynamicRole(long instanceID, string dynamicRole, out string assignedTo, int genericKey, string stateName);

        bool AssignDebtCounsellingCaseForGroupOrLoadBalance(long instanceID, int debtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, string state, System.Collections.Generic.List<string> states, bool includeStates, bool courtCase);

        void AssignWorkflowRole(long instanceID, int adUserKey, int offerRoleTypeOrganisationStructureMappingKey, string stateName);

        void AssignWorkflowRoleForADUser(long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, string state);

        bool CloneActiveSecurityFromInstanceForInstance(long parentInstanceID, long instanceID);

        bool DeActiveUsersForInstance(long instanceID, int genericKey, System.Collections.Generic.List<string> dynamicRoles);

        void DeActiveUsersForInstance(long instanceID, int genericKey, System.Collections.Generic.List<string> dynamicRoles, SAHL.Common.Globals.Process process);

        string GetConsultantForInstanceAndRole(long instanceID, string dynamicRole);

        System.Data.DataTable GetCurrentConsultantAndAdmin(long instanceID);

        void GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(long sourceInstanceID, string dynamicRole, int organisationStructureKey, out string assignedUser);

        bool InsertCommissionableConsultant(long instanceID, string adUserName, int genericKey, string stateName);

        bool IsUserActive(int adUserKey);

        bool IsUserActive(string adUserName);

        bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, string dynamicRole, long instanceID);

        bool ReactivateUserOrLoadBalanceAssign(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName);

        bool ReactivateUserOrLoadBalanceAssign(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName, bool includeStates);

        string ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey);

        bool ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string stateName);

        string ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey);

        bool ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName);

        void ReassignCaseToUser(long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName);

        void ReassignParentMapToCurrentUser(long instanceID, long sourceInstanceID, int applicationKey, string state, SAHL.Common.Globals.Process process);

        string X2LoadBalanceAssign(SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericKeyType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process process, SAHL.Common.Globals.Workflow workflow, bool checkRoundRobinStatus);

        string X2RoundRobinForGivenOSKeys(string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string stateName);

        string X2RoundRobinForGivenOSKeys(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName);

        string X2RoundRobinForPointerDescription(long instanceID, int roundRobinPointerKey, int genericKey, string dynamicRole, string state, SAHL.Common.Globals.Process process);

        int GetBranchManagerOrgStructureKey(long instanceID);

        string AssignBranchManagerForOrgStrucKey(long instanceID, string dynamicRole, int osKey, int genericKey, string state, SAHL.Common.Globals.Process process);

        string GetLatestUserAcrossInstances(long InstanceID, int ApplicationKey, int OSKey, string DynamicRole, string State, SAHL.Common.Globals.Process pName);

        string ResolveDynamicRoleToUserName(string DynamicRole, long InstanceID);

        void ReassignCaseToUser(long InstanceID, int GenercKey, string ADUser, int OSKey, int ORTKey, string State, SAHL.Common.Globals.Process pName);

        string GetLastUserToWorkOnCaseAcrossInstances(long InstanceID, long SourceInstanceID, int ORTKey, string DynamicRole, string MapName);

        string InsertInternetLeadWorkflowAssignment(long InstanceID, int ApplicationKey, string State);

        void ReassignToPreviousValuationsUserIfExistsElseRoundRobin(string DynamicRole, int OrgStructKey, int ApplicationKey, string Map, long InstanceID, string State, int RoundRobinPointerKey);

        /// <summary>
        /// Check if the User is still in the Workflow Role
        /// </summary>
        /// <param name="aDUserName"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <returns></returns>
        bool CheckUserInWorkflowRole(string aDUserName, int workflowRoleTypeKey);

        void DeactivateAllWorkflowRoleAssigmentsForInstance(long instanceID);

        /// <summary>
        /// Much horrible badness lives here that must be exorcised
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        [System.Obsolete("BADNESS, PESILLENCE, DEATH AND DISHONOUR TO ALL THOSE WHO WROTE THIS. DO NOT USE THIS, GOOD LUCK REFACTORING!")]
        bool CreditDecisionCheckAuthorisationRules(int ApplicationKey, long InstanceID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="InstanceID"></param>
        /// <param name="loadBalanceStates"></param>
        /// <param name="loadBalanceIncludeStates"></param>
        /// <param name="loadBalance1stPass"></param>
        /// <param name="loadBalance2ndPass"></param>
        [System.Obsolete("DO NOT USE THIS, NEEDS SERIOUS REFACTORING!")]
        void PerformCreditMandateCheck(int ApplicationKey, System.Int64 InstanceID, System.Collections.Generic.List<string> loadBalanceStates, bool loadBalanceIncludeStates, bool loadBalance1stPass, bool loadBalance2ndPass);

        bool IsPolicyOverride(long InstanceID, long SourceInstanceID, int GenericKey);

        string PolicyOverrideReassignToFirstUserOrRoundRobin(long InstanceID, long SourceInstanceID, int GenericKey, string State, SAHL.Common.Globals.Process pName);

        string ReActivateBranchUsersForOrigination(long AppManIID, long AppCapIID, int ApplicationKey, string State, SAHL.Common.Globals.Process pName);

        string ReactivateLastUserToWorkOnCaseIfValid(string DynamicRole, int GenericKey, int OSKey, long InstanceID, string State);

        string GetUserWhoWorkedOnThisLegalEntitysOtherCasesForDynamicRole(int OfferRoleTypeKey, int ApplicationKey, long InstanceID);

        string HandleApplicationManagamentRolesOnReturnFromNTUtoPreviousState(long InstanceID, int ApplicationKey, string PreNTUState, bool IsFL, long AppCapIID, SAHL.Common.Globals.Process pName);

        bool IsUserInOrganisationStructureRole(string adUserName, IList<OfferRoleTypes> offerRoleTypes);

        string ReturnPolicyOverrideUser(long InstanceID);

        string ReturnFeedbackOnverrideUser(long InstanceID);

        string ResolveWorkflowRoleAssignment(long InstanceID, WorkflowRoleTypes workflowRoleType, WorkflowRoleTypeGroups workflowRoleTypeGroup);

        string RoundRobinAndAssignOtherFLCases(int applicationKey, string dynamicRole, int orgStructureKey, long instanceID, string state, int roundRobinPointerKey);

        IEventList<IADUser> GetAdUsersByWorkflowRoleTypeKey(int workflowRoleTypeKey);

        IRoundRobinPointer DetermineRoundRobinPointerByOfferRoleTypeAndOrgStructure(OfferRoleTypes offerRoleType, int organisationStructureKey);
    }
}