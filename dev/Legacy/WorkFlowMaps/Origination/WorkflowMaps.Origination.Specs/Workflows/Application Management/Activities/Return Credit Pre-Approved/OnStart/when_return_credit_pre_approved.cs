using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Return_Credit_Pre_Approved.OnStart
{
    [Subject("Activity => Return_Credit_Pre_Approved => OnStart")]
    internal class when_return_credit_pre_approved : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.RequireValuation = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Return_Credit_Pre_Approved(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_require_valuation_data_property_to_true = () =>
        {
            workflowData.RequireValuation.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}