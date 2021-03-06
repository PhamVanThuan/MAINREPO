using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Request_Lightstone_Valuation.OnGetStageTransition
{
    [Subject("Activity => Request_Lightstone_Valuation => OnGetStageTransition")] // AutoGenerated
    internal class when_request_lightstone_valuation : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Lightstone_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}