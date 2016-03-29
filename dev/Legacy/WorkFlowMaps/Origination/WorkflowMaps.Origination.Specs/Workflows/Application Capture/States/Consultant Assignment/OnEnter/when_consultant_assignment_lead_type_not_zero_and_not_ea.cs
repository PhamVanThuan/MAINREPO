using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Consultant_Assignment.OnEnter
{
    [Subject("State => Consultant_Assignment => OnEnter")]
    internal class when_consultant_assignment_lead_type_not_zero_and_not_ea : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IWorkflowAssignment assign;
        private static string assignedTo;
        private static string expectedName;
        private static int callCount;

        private Establish context = () =>
        {
            result = false;
            assign = An<IWorkflowAssignment>();
            workflowData.LeadType = 1;
            workflowData.IsEA = false;
            assignedTo = string.Empty;
            expectedName = "test";

            assign.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD,
                out assignedTo, workflowData.ApplicationKey, "Consultant Assignment"))
                .OutRef(expectedName)
                .Return(true)
                .WhenCalled((y) => { callCount++; });

            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assign);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Consultant_Assignment(instanceData, workflowData, paramsData, messages);
        };

        private It should_assign_creator_as_dynamic_role = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}