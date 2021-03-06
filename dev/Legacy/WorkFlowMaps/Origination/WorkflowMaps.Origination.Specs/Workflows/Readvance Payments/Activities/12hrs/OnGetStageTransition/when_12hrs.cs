using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities._12hrs.OnGetStageTransition
{
    [Subject("Activity => 12hrs => OnGetStageTransition")] // AutoGenerated
    internal class when_12hrs : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_12hrs(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}