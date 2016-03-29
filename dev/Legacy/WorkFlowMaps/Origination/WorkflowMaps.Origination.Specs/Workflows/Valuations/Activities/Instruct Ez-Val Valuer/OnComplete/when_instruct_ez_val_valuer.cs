using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Instruct_Ez_Val_Valuer.OnComplete
{
    [Subject("Activity => Instruct_Ez_Val_Valuer => OnComplete")]
    internal class when_instruct_ez_val_valuer : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            message = String.Empty;
            result = false;
            workflowData.IsReview = true;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Instruct_Ez_Val_Valuer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_valuation_isreview_data_property_to_false = () =>
        {
            workflowData.IsReview.ShouldBeFalse();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}