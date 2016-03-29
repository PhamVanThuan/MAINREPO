﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Reallocate_User.OnComplete
{
    [Subject("Activity => Reallocate_User => OnComplete")]
    internal class when_can_reallocate_user : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static SAHL.Common.Globals.WorkflowRoleTypes roleType;

        private Establish context = () =>
        {
            wfa = An<IWorkflowAssignment>();
            result = false;
            workflowData.AssignWorkflowRoleTypeKey = 1;
            workflowData.AssignADUserName = "test";
            workflowData.DebtCounsellingKey = 1;
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            roleType = (SAHL.Common.Globals.WorkflowRoleTypes)workflowData.AssignWorkflowRoleTypeKey;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reallocate_User(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_assign_the_workflow_role_according_to_data_property = () =>
        {
            wfa.WasToldTo(x => x.AssignWorkflowRoleForADUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.AssignADUserName,
                roleType, workflowData.DebtCounsellingKey, ""));
        };
    }
}