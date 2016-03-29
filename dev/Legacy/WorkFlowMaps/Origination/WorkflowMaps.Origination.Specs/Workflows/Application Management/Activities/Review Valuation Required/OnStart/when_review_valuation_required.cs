using Machine.Specifications;
using System.Linq;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Valuation_Required.OnStart
{
    [Subject("Activity => Review_Valuation_Required => OnStart")]
    internal class when_review_valuation_required : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.RequireValuation = true;
        };

        private Because of = () =>
        {
            result
                = workflow.OnStartActivity_Review_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_add_domainservice_message = () =>
        {
            messages.AllMessages.Count().Equals(0);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}