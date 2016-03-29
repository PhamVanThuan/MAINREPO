using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Escalate_to_Manager.OnComplete
{
    [Subject("Activity => Escalate_to_Manager => OnComplete")]
    internal class when_escalate_to_manger : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static int expectedManagerOSKey;
        private static IWorkflowAssignment assignment;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            expectedManagerOSKey = 1;
            dys = new List<string>(){ "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D" };
            workflowData.ApplicationKey = 2;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>()))
                .Return(expectedManagerOSKey);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Escalate_to_Manager(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactive_users_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_get_branch_manager_org_structure_key = () =>
        {
            assignment.WasToldTo(x => x.GetBranchManagerOrgStructureKey((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_assign_branch_manager_for_org_struc_key = () =>
        {
            assignment.WasToldTo(x => x.AssignBranchManagerForOrgStrucKey((IDomainMessageCollection)messages,
                instanceData.ID,
                "Branch Manager D",
                expectedManagerOSKey,
                workflowData.ApplicationKey,
                "Manager Review",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}