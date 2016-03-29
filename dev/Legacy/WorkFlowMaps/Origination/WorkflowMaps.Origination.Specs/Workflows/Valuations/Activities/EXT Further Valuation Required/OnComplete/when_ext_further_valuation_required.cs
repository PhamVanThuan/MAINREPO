using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.EXT_Further_Valuation_Required.OnComplete
{
    [Subject("Activity => EXT_Further_Valuation_Required => OnComplete")]
    internal class when_ext_further_valuation_required : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            message = String.Empty;
            result = false;
            workflowData.EntryPath = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Further_Valuation_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_entry_path_data_property_to_further_valuation_required = () =>
        {
            workflowData.EntryPath.ShouldEqual<int>(3);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}