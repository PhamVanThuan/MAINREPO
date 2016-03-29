using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_Request.OnComplete
{
    [Subject("Activity => Valuation_Request => OnComplete")]
    internal class when_valuation_request : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            workflowData.EntryPath = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Valuation_Request(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_entry_path_data_property_to_valuation_request = () =>
        {
            workflowData.EntryPath.ShouldEqual<int>(2);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}