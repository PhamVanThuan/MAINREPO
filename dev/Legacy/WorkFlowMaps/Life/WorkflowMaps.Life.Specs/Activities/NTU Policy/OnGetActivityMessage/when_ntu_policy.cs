using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.NTU_Policy.OnGetActivityMessage
{
    [Subject("Activity => NTU_Policy => OnGetActivityMessage")] // AutoGenerated
    internal class when_ntu_policy : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_NTU_Policy(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}