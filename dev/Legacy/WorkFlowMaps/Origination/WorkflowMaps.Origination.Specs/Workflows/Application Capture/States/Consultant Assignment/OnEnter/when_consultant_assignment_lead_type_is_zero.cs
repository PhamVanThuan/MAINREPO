using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Consultant_Assignment.OnEnter
{
    [Subject("State => Consultant_Assignment => OnEnter")]
    internal class when_consultant_assignment_lead_type_is_zero : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string expectedName, assignedTo;
        private static int callCount;
        private static IWorkflowAssignment assign;

        private Establish context = () =>
        {
            result = false;
            expectedName = string.Empty;
            assignedTo = string.Empty;
            workflowData.LeadType = 0;
            workflowData.IsEA = false;
            assign = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assign);

            assign.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD,
                          out assignedTo, workflowData.ApplicationKey, "Consultant Assignment"))
                  .OutRef(expectedName)
                  .Return(true)
                  .WhenCalled((y) => { callCount++; });
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Consultant_Assignment(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_assign_creator_as_dynamic_role = () =>
        {
            callCount.ShouldEqual(0);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}