using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.New_Purchase.OnStart
{
    [Subject("Activity => New_Purchase => OnStart")]
    internal class when_offer_type_is_not_new_purchase_loan : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.SwitchLoan;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_New_Purchase(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}