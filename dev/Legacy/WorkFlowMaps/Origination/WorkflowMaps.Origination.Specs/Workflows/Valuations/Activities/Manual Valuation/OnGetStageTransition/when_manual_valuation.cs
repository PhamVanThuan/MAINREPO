using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Manager_Archive.OnGetStageTransition
{
    [Subject("Activity => Manual_Valuation => OnGetStageTransition")]
    internal class when_manual_valuation : WorkflowSpecValuations
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Manual_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_manual_valuation_stagetransition = () =>
        {
            result.ShouldEqual<string>("Manual Valuation");
        };
    }
}