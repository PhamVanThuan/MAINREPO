using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Further_Valuation_Required.OnGetStageTransition
{
    [Subject("Activity => Further_Valuation_Required => OnGetStageTransition")]
    internal class when_further_valuation_required : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Further_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_further_valuation_required_stagetransition = () =>
        {
            result.ShouldEqual<string>("Further Valuation Required");
        };
    }
}