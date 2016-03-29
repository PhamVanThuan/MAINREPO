using Machine.Specifications;
using System;

namespace WorkflowMaps.Valuations.Specs.Activities.Request_Automated_Valuation.OnGetStageTransition
{
    [Subject("Activity => Request_Automated_Valuation => OnGetStageTransition")]
    internal class when_request_automated_valuation : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Automated_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_request_automated_valuation_stagetransition = () =>
        {
            result.ShouldEqual<string>("Request Automated Valuation");
        };
    }
}