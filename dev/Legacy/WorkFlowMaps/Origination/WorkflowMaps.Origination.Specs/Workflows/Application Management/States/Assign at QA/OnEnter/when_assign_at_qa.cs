using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Assign_at_QA.OnEnter
{
    [Subject("State => Assign_at_QA => OnEnter")] // AutoGenerated
    internal class when_assign_at_qa : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Assign_at_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}