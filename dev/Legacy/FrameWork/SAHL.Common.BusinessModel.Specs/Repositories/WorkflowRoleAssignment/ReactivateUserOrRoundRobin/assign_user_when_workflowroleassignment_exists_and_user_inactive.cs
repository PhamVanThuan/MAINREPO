using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Repositories;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Globals;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;


namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowRoleAssignment.ReactivateUserOrRoundRobin
{
    [Subject(typeof(WorkflowRoleAssignmentRepository))]
    public class assign_user_when_workflowroleassignment_exists_and_user_inactive : WorkflowRoleAssignmentRepositoryWithFakesBase
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

            workflowSecurityRepo.WhenToldTo(x=> x.IsUserActive(Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>())).Return(false);
        };

        Because of = () =>
        {
            workflowRoleAssignmentRepo.ReactivateUserOrRoundRobin(Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>());
        };

        It should_round_robin_assign = () =>
        {
            workflowSecurityRepo.WasToldTo(x => x.RoundRobinAssignForPointer(Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>(), Param.IsAny<int>(), Param.IsAny<WorkflowRoleTypes>()));
        };
    }
}
