using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Common_Branch_Rework_.OnEnter
{
    [Subject("State => Common_Branch_Rework_ => OnEnter")] // AutoGenerated
    internal class when_common_branch_rework_ : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Branch_Rework_(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}