using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Set_Active_Valuation.OnGetActivityMessage
{
    [Subject("Activity => Set_Active_Valuation => OnGetActivityMessage")] // AutoGenerated
    internal class when_set_active_valuation : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Set_Active_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}