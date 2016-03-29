using System.Collections.Generic;
using DomainService2.SharedServices.WorkflowAssignment;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using X2DomainService.Interface.WorkflowAssignment;

namespace DomainService2.Hosts.WorkflowAssignment
{
    public class WorkflowAssignmentHost : HostBase, IWorkflowAssignment
    {
        public WorkflowAssignmentHost(ICommandHandler commandHandler)
            : base(commandHandler)
        {
        }

        public bool AssignCreateorAsDynamicRole(IDomainMessageCollection messages, long instanceID, string dynamicRole, out string assignedTo, int genericKey, string stateName)
        {
            var command = new AssignCreateorAsDynamicRoleCommand(instanceID, dynamicRole, genericKey, stateName);
            this.CommandHandler.HandleCommand(messages, command);
            assignedTo = command.AssignedTo;
            return command.Result;
        }

        public bool AssignDebtCounsellingCaseForGroupOrLoadBalance(IDomainMessageCollection messages, long instanceID, int debtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, string state, List<string> states, bool includeStates, bool courtCase)
        {
            var command = new AssignDebtCounsellingCaseForGroupOrLoadBalanceCommand(instanceID, debtCounsellingKey, workflowRoleType, state, states, includeStates, courtCase);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void AssignWorkflowRole(IDomainMessageCollection messages, long instanceID, int adUserKey, int offerRoleTypeOrganisationStructureMappingKey, string stateName)
        {
            var command = new AssignWorkflowRoleCommand(instanceID, adUserKey, offerRoleTypeOrganisationStructureMappingKey, stateName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void AssignWorkflowRoleForADUser(IDomainMessageCollection messages, long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, string state)
        {
            AssignWorkflowRoleForADUserCommand command = new AssignWorkflowRoleForADUserCommand(instanceID, adUserName, workflowRoleType, genericKey, state);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CloneActiveSecurityFromInstanceForInstance(IDomainMessageCollection messages, long parentInstanceID, long instanceID)
        {
            var command = new CloneActiveSecurityFromInstanceForInstanceCommand(parentInstanceID, instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool DeActiveUsersForInstance(IDomainMessageCollection messages, long instanceID, int genericKey, List<string> dynamicRoles)
        {
            var command = new DeActiveUsersForInstanceCommand(instanceID, genericKey, dynamicRoles);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void DeActiveUsersForInstanceAndProcess(IDomainMessageCollection messages, long instanceID, int genericKey, List<string> dynamicRoles, SAHL.Common.Globals.Process process)
        {
            var command = new DeActiveUsersForInstanceAndProcessCommand(instanceID, genericKey, dynamicRoles, process);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public string GetConsultantForInstanceAndRole(IDomainMessageCollection messages, long instanceID, string dynamicRole)
        {
            var command = new GetConsultantForInstanceAndRoleCommand(instanceID, dynamicRole);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public System.Data.DataTable GetCurrentConsultantAndAdmin(IDomainMessageCollection messages, long instanceID)
        {
            var command = new GetCurrentConsultantAndAdminCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(IDomainMessageCollection messages, long sourceInstanceID, string dynamicRole, int organisationStructureKey, out string assignedUser)
        {
            var command = new GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStructCommand(sourceInstanceID, dynamicRole, organisationStructureKey);
            this.CommandHandler.HandleCommand(messages, command);
            assignedUser = command.AssignedUser;
        }

        public bool InsertCommissionableConsultant(IDomainMessageCollection messages, long instanceID, string adUserName, int genericKey, string stateName)
        {
            var command = new InsertCommissionableConsultantCommand(instanceID, adUserName, genericKey, stateName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsUserActiveByADUserKey(IDomainMessageCollection messages, int adUserKey)
        {
            var command = new IsUserActiveByADUserKeyCommand(adUserKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsUserActiveByADUserName(IDomainMessageCollection messages, string adUserName)
        {
            var command = new IsUserActiveByADUserNameCommand(adUserName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool IsUserStillInSameOrgStructureForCaseReassign(IDomainMessageCollection messages, int adUserKey, string dynamicRole, long instanceID)
        {
            var command = new IsUserStillInSameOrgStructureForCaseReassignCommand(adUserKey, dynamicRole, instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ReactivateUserOrLoadBalanceAssign(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName)
        {
            var command = new ReactivateUserOrLoadBalanceAssignCommand(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public bool ReactivateUserOrLoadBalanceAssignWithStates(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, SAHL.Common.Globals.Process processName, SAHL.Common.Globals.Workflow workflowName, bool includeStates)
        {
            var command = new ReactivateUserOrLoadBalanceAssignWithStatesCommand(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName, includeStates);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void ReassignCaseToUser(IDomainMessageCollection messages, long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName)
        {
            var command = new ReassignCaseToUserCommand(instanceID, genericKey, adUserName, organisationStructureKey, offerRoleTypeKey, stateName);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public void ReassignParentMapToCurrentUser(IDomainMessageCollection messages, long instanceID, long sourceInstanceID, int applicationKey, string state, SAHL.Common.Globals.Process process)
        {
            var command = new ReassignParentMapToCurrentUserCommand(instanceID, sourceInstanceID, applicationKey, state, process);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public string X2LoadBalanceAssign(IDomainMessageCollection messages, SAHL.Common.Globals.GenericKeyTypes userOrganisationStructureGenericKeyType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, SAHL.Common.Globals.Process process, SAHL.Common.Globals.Workflow workflow, bool checkRoundRobinStatus)
        {
            var command = new X2LoadBalanceAssignCommand(userOrganisationStructureGenericKeyType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, process, workflow, checkRoundRobinStatus);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string X2RoundRobinForGivenOSKeys(IDomainMessageCollection messages, string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName)
        {
            var command = new X2RoundRobinForGivenOSKeysCommand(dynamicRole, genericKey, organisationStructureKey, instanceID, stateName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string X2RoundRobinForPointerDescription(IDomainMessageCollection messages, long instanceID, int roundRobinPointerKey, int genericKey, string dynamicRole, string state, SAHL.Common.Globals.Process process)
        {
            var command = new X2RoundRobinForPointerDescriptionCommand(instanceID, roundRobinPointerKey, genericKey, dynamicRole, state, process);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string ReactiveUserOrRoundRobinForOSKeysByProcess(IDomainMessageCollection messages, string dynamicRole, int genericKey, List<int> organisationStructureKeys, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey)
        {
            var command = new ReactiveUserOrRoundRobinForOSKeysByProcessCommand(dynamicRole, genericKey, organisationStructureKeys, instanceID, state, process, roundRobinPointerKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string ReactiveUserOrRoundRobinForOSKeyByProcess(IDomainMessageCollection messages, string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey)
        {
            var command = new ReactiveUserOrRoundRobinForOSKeyByProcessCommand(dynamicRole, genericKey, organisationStructureKey, instanceID, state, process, roundRobinPointerKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string X2RoundRobinForGivenOSKey(IDomainMessageCollection messages, string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName)
        {
            var command = new X2RoundRobinForGivenOSKeyCommand(dynamicRole, genericKey, organisationStructureKey, instanceID, stateName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public int GetBranchManagerOrgStructureKey(IDomainMessageCollection messages, long instanceID)
        {
            var command = new GetBranchManagerOrgStructureKeyCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.OrganisationStructureKeyResult;
        }

        public string AssignBranchManagerForOrgStrucKey(IDomainMessageCollection messages, long instanceID, string dynamicRole, int organisationStructureKey, int genericKey, string state, SAHL.Common.Globals.Process process)
        {
            var command = new AssignBranchManagerForOrgStrucKeyCommand(instanceID, dynamicRole, organisationStructureKey, genericKey, state, process);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedManagerResult;
        }

        public string ResolveDynamicRoleToUserName(IDomainMessageCollection messages, string DynamicRole, long InstanceID)
        {
            var command = new ResolveDynamicRoleToUserNameCommand(DynamicRole, InstanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public void ReassignCaseToUserByProcess(IDomainMessageCollection messages, long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName, SAHL.Common.Globals.Process process)
        {
            var command = new ReassignCaseToUserByProcessCommand(instanceID, genericKey, adUserName, organisationStructureKey, offerRoleTypeKey, stateName, process);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public string GetLastUserToWorkOnCaseAcrossInstances(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int offerRoleTypeKey, string DynamicRole, string MapName)
        {
            var command = new GetLastUserToWorkOnCaseAcrossInstancesCommand(InstanceID, SourceInstanceID, offerRoleTypeKey, DynamicRole, MapName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public string InsertInternetLeadWorkflowAssignment(IDomainMessageCollection messages, long instanceID, int genericKey, string state)
        {
            var command = new InsertInternetLeadWorkflowAssignmentCommand(instanceID, genericKey, state);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedToResult;
        }

        public string GetLatestUserAcrossInstances(IDomainMessageCollection messages, long InstanceID, int ApplicationKey, int OrganisationStructureKey, string DynamicRole, string State, SAHL.Common.Globals.Process pName)
        {
            var command = new GetLatestUserAcrossInstancesCommand(InstanceID, ApplicationKey, OrganisationStructureKey, DynamicRole, State, pName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public void ReassignToPreviousValuationsUserIfExistsElseRoundRobin(IDomainMessageCollection messages, string DynamicRole, int OrgStructKey, int ApplicationKey, string Map, long InstanceID, string State, int RoundRobinPointerKey)
        {
            var command = new ReassignToPreviousValuationsUserIfExistsElseRoundRobinCommand(DynamicRole, OrgStructKey, ApplicationKey, Map, InstanceID, State, RoundRobinPointerKey);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool CheckUserInWorkflowRole(IDomainMessageCollection messages, string adUserName, int workflowRoleTypeKey)
        {
            CheckUserInWorkflowRoleCommand command = new CheckUserInWorkflowRoleCommand(adUserName, workflowRoleTypeKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public void DeactivateAllWorkflowRoleAssigmentsForInstance(IDomainMessageCollection messages, long instanceID)
        {
            DeactivateAllWorkflowRoleAssigmentsForInstanceCommand command = new DeactivateAllWorkflowRoleAssigmentsForInstanceCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
        }

        public bool IsPolicyOverride(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int GenericKey)
        {
            var command = new IsPolicyOverrideCommand(InstanceID, SourceInstanceID, GenericKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }

        public string PolicyOverrideReassignToFirstUserOrRoundRobin(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int GenericKey, string State, SAHL.Common.Globals.Process pName)
        {
            var command = new PolicyOverrideReassignToFirstUserOrRoundRobinCommand(InstanceID, SourceInstanceID, GenericKey, State, pName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUserResult;
        }

        public string AssignCaseThatWasPreviouslyInDisputeIndicated(IDomainMessageCollection messages, int offerKey, long instanceID)
        {
            var command = new AssignCaseThatWasPreviouslyInDisputeIndicatedCommand(offerKey, instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedToResult;
        }

        public void GetFirstAssignedCreditUser(IDomainMessageCollection messages, long SourceInstanceID, out string adUserName, out int offerRoleTypeKey, out int organisationStructureKey)
        {
            var command = new GetFirstAssignedCreditUserCommand(SourceInstanceID);
            this.CommandHandler.HandleCommand(messages, command);
            adUserName = command.ADUserName;
            offerRoleTypeKey = command.OfferRoleTypeKey;
            organisationStructureKey = command.OrganisationStructureKey;
        }

        public string ReassignToMostSenPersonWhoWorkedOnThisCaseInCredit(IDomainMessageCollection messages, long InstanceID, long SourceInstanceID, int OfferKey, string State)
        {
            var command = new ReassignToMostSenPersonWhoWorkedOnThisCaseInCreditCommand(InstanceID, SourceInstanceID, OfferKey, State);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUserResult;
        }

        public string ReActivateBranchUsersForOrigination(IDomainMessageCollection messages, long AppManIID, long AppCapIID, int ApplicationKey, string State, SAHL.Common.Globals.Process pName)
        {
            var command = new ReActivateBranchUsersForOriginationCommand(AppManIID, AppCapIID, ApplicationKey, State, pName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUsersResult;
        }

        public string HandleApplicationManagementRolesOnReturnFromNTUtoPreviousState(IDomainMessageCollection messages, long InstanceID, int ApplicationKey, string PreNTUState, bool IsFL, long AppCapIID, SAHL.Common.Globals.Process pName)
        {
            var command = new HandleApplicationManagementRolesOnReturnFromNTUtoPreviousStateCommand(InstanceID, ApplicationKey, PreNTUState, IsFL, AppCapIID, pName);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUserResult;
        }

        public string GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(IDomainMessageCollection messages, int OfferRoleTypeKey, int ApplicationKey, long InstanceID)
        {
            var command = new GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRoleCommand(OfferRoleTypeKey, ApplicationKey, InstanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public string ResolveWorkflowRoleAssignment(IDomainMessageCollection messages, long instanceID, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, SAHL.Common.Globals.WorkflowRoleTypeGroups workflowRoleTypeGroup)
        {
            var command = new ResolveWorkflowRoleAssignmentCommand(instanceID, workflowRoleType, workflowRoleTypeGroup);
            this.CommandHandler.HandleCommand(messages, command);
            return command.ADUserNameResult;
        }

        public string ReturnPolicyOverrideUser(IDomainMessageCollection messages, long instanceID)
        {
            var command = new ReturnPolicyOverrideUserCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.UserResult;
        }

        public string ReturnFeedbackOnverrideUser(IDomainMessageCollection messages, long instanceID)
        {
            var command = new ReturnFeedbackOnverrideUserCommand(instanceID);
            this.CommandHandler.HandleCommand(messages, command);
            return command.UserResult;
        }

        public string RoundRobinAndAssignOtherFLCases(IDomainMessageCollection messages, int applicationKey, string dynamicRole, int orgStructureKey, long instanceID, string state, int roundRobinPointerKey)
        {
            var command = new RoundRobinAndAssignOtherFLCasesCommand(applicationKey, dynamicRole, orgStructureKey, instanceID, state, roundRobinPointerKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUserResult;
        }

        public string ReactivateUserOrRoundRobinForWorkflowRoleAssignment(IDomainMessageCollection messages, GenericKeyTypes uosGenericKeyType, WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, RoundRobinPointers roundRobinPointer)
        {
            var command = new ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommand(uosGenericKeyType, workflowRoleType, genericKey, instanceID, roundRobinPointer);
            this.CommandHandler.HandleCommand(messages, command);
            return command.AssignedUserResult;
        }

        public int GetRoundRobinPointer(IDomainMessageCollection messages, OfferRoleTypes offerRole, int organisationStructureKey)
        {
            var command = new GetRoundRobinPointerCommand(offerRole, organisationStructureKey);
            this.CommandHandler.HandleCommand(messages, command);
            return command.Result;
        }
    }
}