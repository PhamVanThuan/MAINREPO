﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Roles.Recoveries_Manager_D.OnGetRole
{
    [Subject("Roles => Recoveries_Manager_D => OnGetRole")]
    internal class when_recoveries_manager_d : WorkflowSpecDebtCounselling
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
            result = workflow.OnGetRole_Debt_Counselling_Recoveries_Manager_D(instanceData, workflowData, roleName, paramsData, messages);
        };

        private It should_resolved_workflow_role_assignment_for_recoveries_manager_d = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}