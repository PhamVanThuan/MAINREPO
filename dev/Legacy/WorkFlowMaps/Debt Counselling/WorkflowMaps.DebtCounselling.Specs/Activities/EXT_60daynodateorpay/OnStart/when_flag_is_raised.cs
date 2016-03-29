using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.EXT_60daynodateorpay.OnStart
{
    [Subject("Activity => EXT_60daynodateorpay => OnStart")]
    internal class when_flag_is_raised : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string state;

        private Establish context = () =>
            {
                state = "State";
                ((ParamsDataStub)paramsData).StateName = state;
                result = false;
            };

        private Because of = () =>
            {
                result = workflow.OnStartActivity_EXT_60daynodateorpay(instanceData, workflowData, paramsData, messages);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_set_the_previous_state_data_property_to_the_instance_data_statename = () =>
            {
                workflowData.PreviousState.ShouldEqual(state);
            };
    }
}