using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Assign_Credit.OnExit
{
    [Subject("State => Assign_Credit => OnExit")]
    internal class when_assign_credit : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Assign_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_exceptions_decline_with_offer_property_to_false = () =>
        {
            workflowData.ExceptionsDeclineWithOffer.ShouldBeFalse();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}