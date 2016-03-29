using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Lead___OfferType_NONE.OnStart
{
    [Subject("Activity => Lead___OfferType_NONE => OnStart")]
    internal class when_lead_type_is_0_and_not_assigned_to_branch_consultant_d_or_branch_manager_d : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int callCount;

        private Establish context = () =>
        {
            result = false;
            workflowData.LeadType = 0;
            callCount = 0;
            var assignment = An<IWorkflowAssignment>();
            string assignedTo;
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "Manage Lead"))
                .OutRef(string.Empty)
                .Return(false)
                .IgnoreArguments()
                .Repeat.Twice();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD, out assignedTo, workflowData.ApplicationKey, "Manage Lead"))
                .OutRef("Test")
                .Return(true)
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Lead___OfferType_NONE(instanceData, workflowData, paramsData, messages);
        };

        private It should_assign_creator_as_dynamic_role_for_branch_admin_d = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}