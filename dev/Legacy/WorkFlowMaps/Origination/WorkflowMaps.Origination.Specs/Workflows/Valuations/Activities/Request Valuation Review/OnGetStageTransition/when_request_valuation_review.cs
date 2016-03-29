using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Request_Valuation_Review.OnGetStageTransition
{
    [Subject("Activity => Request_Valuation_Review => OnGetStageTransition")]
    internal class when_request_valuation_review : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Request_Valuation_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_request_valuation_review_stagetransition = () =>
        {
            result.ShouldEqual<string>("Request Valuation Review");
        };
    }
}