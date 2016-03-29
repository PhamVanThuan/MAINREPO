using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowRoleAssignment.ReactivateUserOrRoundRobin
{
    [Subject(typeof(WorkflowRoleAssignmentRepository))]
    public class assign_user_when_workflowroleassignment_exists_and_user_not_in_org_struct : WorkflowRoleAssignmentRepositoryWithFakesBase
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

            workflowSecurityRepo.WhenToldTo(x => x.IsUserStillInSameOrgStructureForCaseReassign(Param.IsAny<int>(), Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>())).Return(false);
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
