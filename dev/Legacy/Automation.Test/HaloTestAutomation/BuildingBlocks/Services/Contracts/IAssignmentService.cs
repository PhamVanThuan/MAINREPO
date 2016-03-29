using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IAssignmentService
    {
        string GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum offerRoleTypeKey, RoundRobinPointerEnum roundRobinPointer);

        Dictionary<int, string> GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum workflowRoleType, int genericKey);

        QueryResults GetActiveWorkflowRoleAssignmentByInstance(Int64 instanceID);

        string GetDebtCounsellingGroupAssign(WorkflowRoleTypeEnum wrt, int debtCounsellingKey);

        string GetLoadBalanceUserAssign(WorkflowRoleTypeEnum wrt, int workflowID, string states, bool incl);

        QueryResults GetActiveWorkflowRoleByAdUserAndType(int genericKey, WorkflowRoleTypeEnum wfRoleType, string adUserName);

        string GetNextLoadBalanceAssignment(WorkflowRoleTypeEnum wrt, string workflowName, IList<string> states, bool incl);

        string GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum wrt, string workflowName, IList<string> states, bool incl, int debtCounsellingKey);

        string GetADUserNameOfActiveWorkflowRoleAssignment(Int64 instanceID);

        bool OfferRoleTypesAssignedInX2WorkFlowAssignment(int offerKey, string stateName, string offerRoleTypeKey);

        QueryResults GetLatestWorkFlowAssignment(int offerKey, OfferRoleTypeEnum offerRoleType);

        QueryResults GetActiveOfferRolesByOfferRoleType(int offerkey, OfferRoleTypeEnum offerRoleType);

        IEnumerable<Automation.DataModels.X2WorkflowList> GetWorklistDetails(int offerKey, string state, string workflow);

        QueryResults GetWorkflowRoleAssignmentByInstance(Int64 instanceID);

        QueryResults GetX2WorkFlowAssignment_ByStateName(int offerkey, string stateName);

        QueryResults GetAdUsersForWorkflowRoleType(string adUserName);

        IEnumerable<Automation.DataModels.X2WorkflowList> GetWorklistDetails(string adusername, string workflowMapName);

        string GetNextRoundRobinAssignmentByWorkflowRoleTypeKey(WorkflowRoleTypeEnum workflowRoleTypeKey, RoundRobinPointerEnum roundRobinPointer);

        Dictionary<int, string> GetActiveWorkflowRoleByTypeAndGenericKeyForRoundRobinAssign(WorkflowRoleTypeEnum workflowRoleType, int genericKey);

        string GetUserForReactivateOrRoundRobinAssignment(int generickey, WorkflowRoleTypeEnum workflowRoleType, RoundRobinPointerEnum roundRobinPointer);

        QueryResults GetActiveWorkflowRolesByADUser(string aduser);

        QueryResults GetAdUsersForWorkflowRoleType(WorkflowRoleTypeEnum workflowRoleType);

        QueryResults GetWorkflowAssignmentByInstance(Int64 instanceID);

        string GetNextRoundRobinUser(OfferRoleTypeEnum offerRoleTypeEnum, RoundRobinPointerEnum roundRobinPointerEnum);
    }
}