using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.X2.Common;

namespace X2DomainService.Interface.WorkflowAssignment
{
    public interface IWorkflowAssignment : IX2WorkflowService
    {
        void GetFirstAssignedCreditUser(IDomainMessageCollection messages, long SourceInstanceID, out string adUserName, out int offerRoleTypeKey, out int orgStructureKey);

        bool AssignCreateorAsDynamicRole(IDomainMessageCollection messages, long instanceID, string dynamicRole, out string assignedTo, int genericKey, string stateName);

        bool AssignDebtCounsellingCaseForGroupOrLoadBalance(IDomainMessageCollection messages, long instanceID, int debtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, string state, System.Collections.Generic.List<string> states, bool includeStates, bool courtCase);

        void AssignWorkflowRole(IDomainMessageCollection messages, long instanceID, int adUserKey, int offerRoleTypeOrganisationStructureMappingKey, string stateName);

        void AssignWorkflowRoleForADUser(IDomainMessageCollection messages, long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, string state);

        bool CloneActiveSecurityFromInstanceForInstance(IDomainMessageCollection messages, long parentInstanceID, long instanceID);

        bool DeActiveUsersForInstance(IDomainMessageCollection messages, long instanceID, int genericKey, System.Collections.Generic.List<string> dynamicRoles);

        void DeActiveUsersForInstanceAndProcess(IDomainMessageCollection messages, long instanceID, int genericKey, System.Collections.Generic.List<string> dynamicRoles, SAHL.Common.Globals.Process process);

        string GetConsultantForInstanceAndRole(IDomainMessageCollection messages, long instanceID, string dynamicRole);

        System.Data.DataTable GetCurrentConsultantAndAdmin(IDomainMessageCollection messages, long instanceID);

        void GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(IDomainMessageCollection messages, long sourceInstanceID, string dynamicRole, int organisationStructureKey, out string assignedUser);

        bool InsertCommissionableConsultant(IDomainMessageCollection messages, long instanceID, string adUserName, int genericKey, string stateName);

        bool IsUserActiveByADUserKey(IDomainMessageCollection messages, int adUserKey);

        bool IsUserActiveByADUserName(IDomainMessageCollection messages, string adUserName);

        bool IsUserStillInSameOrgStructureForCaseReassign(IDomainMessageCollection messages, int adUserKey, string dynamicRole, long instanceID);

        bool ReactivateUserOrLoadBalanceAssign(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName);

        bool ReactivateUserOrLoadBalanceAssignWithStates(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName, bool includeStates);

        string ReactiveUserOrRoundRobinForOSKeysByProcess(IDomainMessageCollection messages, string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey);

        string ReactiveUserOrRoundRobinForOSKeyByProcess(IDomainMessageCollection messages, string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey);

        void ReassignCaseToUser(IDomainMessageCollection messages, long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName);

        void ReassignParentMapToCurrentUser(IDomainMessageCollection messages, long instanceID, long sourceInstanceID, int applicationKey, string state, SAHL.Common.Globals.Process process);

        string X2LoadBalanceAssign(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericKeyType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, System.Collections.Generic.List<string> statesToDetermineLoad, SAHL.Common.Globals.Process process, SAHL.Common.Globals.Workflow workflow, bool checkRoundRobinStatus);

        string X2RoundRobinForGivenOSKey(IDomainMessageCollection messages, string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName);

        string X2RoundRobinForPointerDescription(IDomainMessageCollection messages, long instanceID, int roundRobinPointerKey, int genericKey, string dynamicRole, string state, SAHL.Common.Globals.Process process);

        int GetBranchManagerOrgStructureKey(IDomainMessageCollection messages, long instanceID);

        string AssignBranchManagerForOrgStrucKey(IDomainMessageCollection messages, long instanceID, string dynamicRole, int organisationStructureKey, int genericKey, string state, SAHL.Common.Globals.Process process);

        string ResolveDynamicRoleToUserName(IDomainMessageCollection messages, string DynamicRole, long InstanceID);

        void ReassignCaseToUserByProcess(IDomainMessageCollection messages, long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName, SAHL.Common.Globals.Process process);

        string GetLastUserToWorkOnCaseAcrossInstances(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int offerRoleTypeKey, string DynamicRole, string MapName);

        string InsertInternetLeadWorkflowAssignment(IDomainMessageCollection messages, long InstanceID, int ApplicationKey, string State);

        string GetLatestUserAcrossInstances(IDomainMessageCollection messages, long InstanceID, int ApplicationKey, int OrganisationStructureKey, string DynamicRole, string State, SAHL.Common.Globals.Process pName);

        void ReassignToPreviousValuationsUserIfExistsElseRoundRobin(IDomainMessageCollection messages, string DynamicRole, int OrgStructKey, int ApplicationKey, string Map, long InstanceID, string State, int RoundRobinPointerKey);

        bool CheckUserInWorkflowRole(IDomainMessageCollection messages, string adUserName, int workflowRoleTypeKey);

        void DeactivateAllWorkflowRoleAssigmentsForInstance(IDomainMessageCollection messages, long InstanceID);

        bool IsPolicyOverride(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int GenericKey);

        string PolicyOverrideReassignToFirstUserOrRoundRobin(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int GenericKey, string State, SAHL.Common.Globals.Process pName);

        string AssignCaseThatWasPreviouslyInDisputeIndicated(IDomainMessageCollection messages, int offerKey, long instanceID);

        string ReassignToMostSenPersonWhoWorkedOnThisCaseInCredit(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int OfferKey, string State);

        string HandleApplicationManagementRolesOnReturnFromNTUtoPreviousState(IDomainMessageCollection messages, long InstanceID, int ApplicationKey, string PreNTUState, bool IsFL, long AppCapIID, SAHL.Common.Globals.Process pName);

        string ReActivateBranchUsersForOrigination(IDomainMessageCollection messages, long AppManIID, long AppCapIID, int ApplicationKey, string State, SAHL.Common.Globals.Process pName);

        string GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(IDomainMessageCollection messages, int OfferRoleTypeKey, int ApplicationKey, long InstanceID);

        string ResolveWorkflowRoleAssignment(IDomainMessageCollection messages, long InstanceID, WorkflowRoleTypes workflowRoleType, WorkflowRoleTypeGroups workflowRoleTypeGroup);

        string ReturnPolicyOverrideUser(IDomainMessageCollection messages, long instanceID);

        string ReturnFeedbackOnverrideUser(IDomainMessageCollection messages, long instanceID);

        string RoundRobinAndAssignOtherFLCases(IDomainMessageCollection messages, int applicationKey, string dynamicRole, int orgStructureKey, long instanceID, string state, int roundRobinPointerKey);

        string ReactivateUserOrRoundRobinForWorkflowRoleAssignment(IDomainMessageCollection messages, GenericKeyTypes uosGenericKeyType, WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, RoundRobinPointers roundRobinPointer);

        int GetRoundRobinPointer(IDomainMessageCollection messages, OfferRoleTypes offerRole, int organisationStructureKey);
    }
}