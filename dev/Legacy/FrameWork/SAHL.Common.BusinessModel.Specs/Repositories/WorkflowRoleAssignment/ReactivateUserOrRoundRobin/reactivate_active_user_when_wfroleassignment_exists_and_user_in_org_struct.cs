using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowRoleAssignment.ReactivateUserOrRoundRobin
{
    [Subject(typeof(WorkflowRoleAssignmentRepository))]
    public class reactivate_active_user_when_workflowroleassignment_exists_and_user_in_org_struct : WorkflowRoleAssignmentRepositoryWithFakesBase
    {
        Establish context = () =>
            {
                var row = dataSet.WFRAssignment.NewWFRAssignmentRow();

                row.ADUserKey = 1;
                row.ID = 1;
                row.InstanceID = 1;
                row.BlaKey = 1;
                row.GeneralStatusKey = 1;
                row.ADUserName = "";
                row.ADUserKey = 1;

                dataSet.WFRAssignment.Rows.Add(row);

                workflowSecurityRepo.WhenToldTo(x => x.GetWorkflowRoleAssignment(Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<long>())).Return(dataSet);

                workflowSecurityRepo.WhenToldTo(x => x.IsUserStillInSameOrgStructureForCaseReassign(Param.IsAny<int>(), Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>())).Return(true);

                workflowSecurityRepo.WhenToldTo(x => x.IsUserActive(Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>())).Return(true);

                var adUserRow = dataSet.ADUser.NewADUserRow();
                adUserRow.ADUserKey = 1;
                adUserRow.LegalEntityKey = 1;

                workflowSecurityRepo.WhenToldTo(x => x.GetADUser(Param.IsAny<int>())).Return(adUserRow);
            };

        Because of = () =>
            {
                workflowRoleAssignmentRepo.ReactivateUserOrRoundRobin(Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>());
            };

        It should_deactivate_all_WorkflowRoleAssigments_for_instance = () =>
            {
                workflowSecurityRepo.WasToldTo(x => x.DeactivateAllWorkflowRoleAssigmentsForInstance(Param.IsAny<long>()));
            };

        It should_create_WorkflowRoleAssignment = () =>
            {
                workflowSecurityRepo.WasToldTo(x => x.CreateWorkflowRoleAssignment(Param.IsAny<long>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<string>()));
            };

        It should_deactivate_WorkflowRole_for_the_dynamic_role = () =>
            {
                workflowSecurityRepo.WasToldTo(x => x.DeactivateWorkflowRoleForDynamicRole(Param.IsAny<int>(), Param.IsAny<int>()));
            };

        It should_create_WorkflowRole_in_TwoAM = () =>
            {
                workflowSecurityRepo.WasToldTo(x => x.Create2AMWorkflowRole(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()));
            };
    }
}