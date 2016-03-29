using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Repositories;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowRoleAssignment.ReactivateUserOrRoundRobin
{
    [Subject(typeof(WorkflowRoleAssignmentRepository))]
    public class assign_user_for_RoundRobinPointer : WorkflowRoleAssignmentRepositoryWithFakesBase
    {
        Establish context = () =>
            {
                workflowSecurityRepo.WhenToldTo(x => x.GetWorkflowRoleAssignment(Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<long>())).Return(dataSet);
            };

        Because of = () =>
            {
                workflowRoleAssignmentRepo.ReactivateUserOrRoundRobin(Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>());
            };

        It should_round_robin_assign = () =>
            {
                workflowSecurityRepo.WasToldTo(x=>x.RoundRobinAssignForPointer(Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>(), Param.IsAny<int>(), Param.IsAny<WorkflowRoleTypes>()));
            };
    }
}
