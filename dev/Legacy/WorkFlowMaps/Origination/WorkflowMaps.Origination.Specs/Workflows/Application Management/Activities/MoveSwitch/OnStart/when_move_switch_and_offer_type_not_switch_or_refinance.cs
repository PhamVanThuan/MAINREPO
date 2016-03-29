using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.MoveSwitch.OnStart
{
    [Subject("Activity => MoveSwitch => OnStart")]
    internal class when_move_switch_and_offer_type_not_switch_or_refinance : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_MoveSwitch(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}