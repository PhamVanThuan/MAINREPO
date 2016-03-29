using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Not_Direct_Consultant.OnStart
{
    [Subject("Activity => Not_Direct_Consultant => OnStart")]
    internal class when_lead_type_is_1_is_not_estate_agent_other_and_assigned_to_is_not_empty : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static int callCount;
        private static string assignedToExpected;

        private Establish context = () =>
        {
            result = false;
            workflowData.LeadType = 1;
            workflowData.CaseOwnerName = string.Empty;
            assignedToExpected = "Test";
            callCount = 0;

            var client = An<IApplicationCapture>();
            client.WhenToldTo(x => x.IsEstateAgentInRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);

            assignment = An<IWorkflowAssignment>();
            string assignedTo;
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "UserAssignCheck"))
                .OutRef(assignedToExpected)
                .Return(true)
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Not_Direct_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_perform_assign_creator_as_dynamic_role_for_branch_consultant_d = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_perform_insert_commissionable_consultant_for_assigned_to_user = () =>
        {
            assignment.WasToldTo(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages, instanceData.ID, assignedToExpected, workflowData.ApplicationKey, "UserAssignCheck"));
        };

        private It should_set_case_owner_name_data_property_to_assigned_to_user = () =>
        {
            workflowData.CaseOwnerName.ShouldMatch(assignedToExpected);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}