using System;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Globals;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IWorkflowRoleAssignmentRepository))]
    public class WorkflowRoleAssignmentRepository : IWorkflowRoleAssignmentRepository
    {
        private IWorkflowSecurityRepository workflowSecurityRepository;
        private ICastleTransactionsService castleTransactionService;

        public WorkflowRoleAssignmentRepository(IWorkflowSecurityRepository workflowSecurityRepository, ICastleTransactionsService castleTransactionService)
        {
            this.workflowSecurityRepository = workflowSecurityRepository;
            this.castleTransactionService = castleTransactionService;
        }

        /// <summary>
        /// /Specific to WorkFlow Role Assignment
        /// </summary>
        /// <param name="uosGenericKeyType"></param>
        /// <param name="workflowRoleType"></param>
        ///
        /// <param name="genericKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="roundRobinPointer"></param>
        /// <returns></returns>
        public string ReactivateUserOrRoundRobin(GenericKeyTypes uosGenericKeyType, WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, RoundRobinPointers roundRobinPointer)
        {
            string AssignedUser = string.Empty; ;
            // Get the existing record for this case.
            workflowRole.WorkflowAssignment ds = workflowSecurityRepository.GetWorkflowRoleAssignment(workflowRoleType, instanceID);

            if (ds.WFRAssignment.Rows.Count > 0)
            {
                workflowRole.WorkflowAssignment.WFRAssignmentRow row = (workflowRole.WorkflowAssignment.WFRAssignmentRow)ds.WFRAssignment.Rows[0];
                if (workflowSecurityRepository.IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, uosGenericKeyType, workflowRoleType))
                {
                    if (workflowSecurityRepository.IsUserActive(uosGenericKeyType, workflowRoleType, row.ADUserKey))
                    {
                        // reactivate also make sure the workflowrole record is reactivated
                        AssignedUser = row.ADUserName;

                        // this inserts a record into the x2.workflowroleassignment table
                        workflowSecurityRepository.DeactivateAllWorkflowRoleAssigmentsForInstance(instanceID);
                        workflowSecurityRepository.CreateWorkflowRoleAssignment(instanceID, row.BlaKey, row.ADUserKey, String.Empty);
                        int legalEntityKey = workflowSecurityRepository.GetADUser(row.ADUserKey).LegalEntityKey;

                        // insert / update the [2am]..WorkflowRole
                        workflowSecurityRepository.DeactivateWorkflowRoleForDynamicRole((int)workflowRoleType, genericKey);
                        workflowSecurityRepository.Create2AMWorkflowRole(genericKey, (int)workflowRoleType, legalEntityKey);
                        return AssignedUser;
                    }
                    else
                    {
                        // user is incative - round robin assign here
                        AssignedUser = workflowSecurityRepository.RoundRobinAssignForPointer(instanceID, roundRobinPointer, genericKey, workflowRoleType);
                        return AssignedUser;
                    }
                }
                else
                {
                    // user has moved org structure - round robin assign here
                    AssignedUser = workflowSecurityRepository.RoundRobinAssignForPointer(instanceID, roundRobinPointer, genericKey, workflowRoleType);
                    return AssignedUser;
                }
            }
            else
            {
                // never been assigned  - therefore round robin assign here
                AssignedUser = workflowSecurityRepository.RoundRobinAssignForPointer(instanceID, roundRobinPointer, genericKey, workflowRoleType);
                return AssignedUser;
            }
        }
    }
}