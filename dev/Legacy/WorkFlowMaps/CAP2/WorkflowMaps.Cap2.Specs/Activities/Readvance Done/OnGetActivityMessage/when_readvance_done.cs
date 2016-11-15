using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Readvance_Done.OnGetActivityMessage
{
    [Subject("Activity => Readvance_Done => OnGetActivityMessage")] // AutoGenerated
    internal class when_readvance_done : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Readvance_Done(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}