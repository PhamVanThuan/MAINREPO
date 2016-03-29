using Machine.Specifications;
using System;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Manual_Valuation.OnComplete
{
    [Subject("Activity => Manual_Valuation => OnComplete")]
    internal class when_manual_valuation : WorkflowSpecValuations
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            workflowData.ValuationDataProviderDataServiceKey = 200;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Manual_Valuation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_valuation_data_provider_data_service_property_to_sahl_manual_valuation = () =>
        {
            workflowData.ValuationDataProviderDataServiceKey.ShouldEqual<int>(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}