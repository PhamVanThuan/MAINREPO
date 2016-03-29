using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_in_Order.OnGetStageTransition
{
    [Subject("Activity => Valuation_in_Order => OnGetStageTransition")]
    internal class when_valuation_in_order : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Valuation_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_valuation_in_order_stagetransition = () =>
        {
            result.ShouldEqual<string>("Valuation in Order");
        };
    }
}