using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.MoveSwitch.OnStart
{
    [Subject("Activity => MoveSwitch => OnStart")]
    internal class when_move_switch_and_offer_type_switch_loan : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.SwitchLoan;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_MoveSwitch(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}