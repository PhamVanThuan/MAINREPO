using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Assign_EstateAgent_Lead.OnGetActivityMessage
{
    [Subject("Activity => Assign_EstateAgent_Lead => OnGetActivityMessage")] // AutoGenerated
    internal class when_assign_estateagent_lead : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Assign_EstateAgent_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}