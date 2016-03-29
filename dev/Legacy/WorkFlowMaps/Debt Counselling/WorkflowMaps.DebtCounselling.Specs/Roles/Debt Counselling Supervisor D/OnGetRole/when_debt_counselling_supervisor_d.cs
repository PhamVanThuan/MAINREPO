﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Roles.Debt_Counselling_Supervisor_D.OnGetRole
{
    [Subject("Roles => Debt_Counselling_Supervisor_D => OnGetRole")]
    internal class when_debt_counselling_supervisor_d : WorkflowSpecDebtCounselling
    {
        private static string result;
        private static string expectedResult;
        private static string roleName;

        private Establish context = () =>
        {
            result = string.Empty;
            roleName = string.Empty;
            expectedResult = "Test";
            var client = An<IWorkflowAssignment>();
            client.WhenToldTo(x => x.ResolveWorkflowRoleAssignment(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypeGroups>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnGetRole_Debt_Counselling_Debt_Counselling_Supervisor_D(instanceData, workflowData, roleName, paramsData, messages);
        };

        private It should_resolved_workflow_role_assignment_for_debt_supervisor_consultant_d = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}