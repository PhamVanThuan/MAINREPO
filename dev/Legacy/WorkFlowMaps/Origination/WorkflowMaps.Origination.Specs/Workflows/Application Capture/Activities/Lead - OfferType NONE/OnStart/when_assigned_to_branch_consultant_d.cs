using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Lead___OfferType_NONE.OnStart
{
    [Subject("Activity => Lead___OfferType_NONE => OnStart")]
    internal class when_assigned_to_branch_consultant_d : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int assignCreateorAsDynamicRoleCallCount;
        private static int insertCommissionableConsultantCallCount;
        private static string assignedToExpected;

        private Establish context = () =>
        {
            result = false;
            workflowData.LeadType = 0;
            workflowData.CaseOwnerName = string.Empty;
            assignCreateorAsDynamicRoleCallCount = 0;
            insertCommissionableConsultantCallCount = 0;
            var assignment = An<IWorkflowAssignment>();
            string assignedTo = string.Empty;
            assignedToExpected = "Test";
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "Manage Lead"))
                .OutRef(assignedToExpected)
                .Return(false)
                .WhenCalled((y) => { assignCreateorAsDynamicRoleCallCount++; });
            assignment.Expect(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages, instanceData.ID, assignedToExpected, workflowData.ApplicationKey, "Manage Lead"))
                .Return(false)
                .WhenCalled((y) => { insertCommissionableConsultantCallCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Lead___OfferType_NONE(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_case_owner_name_data_property_to_assigned_to_output = () =>
        {
            workflowData.CaseOwnerName.ShouldMatch(assignedToExpected);
        };

        private It should_assign_creator_as_dynamic_role_for_branch_admin_d = () =>
        {
            assignCreateorAsDynamicRoleCallCount.ShouldEqual(1);
        };

        private It should_perform_insert_commissionable_consultant = () =>
        {
            insertCommissionableConsultantCallCount.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}