using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Awaiting_Confirmation_Timout.OnComplete
{
    [Subject("Activity => Awaiting_Confirmation_Timout => OnComplete")]
    internal class when_not_at_callback_hold_or_ready_to_callback_states : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;
        private static string message;

        private Establish context = () =>
            {
                client = An<ILife>();
                result = true;
                ((ParamsDataStub)paramsData).StateName = "Awaiting Confirmation";
                domainServiceLoader.RegisterMockForType<ILife>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Awaiting_Confirmation_Timout(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_complete_the_awaiting_confirmation_timeout = () =>
            {
                client.WasToldTo(x => x.AwaitingConfirmationTimeout(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
            };

        private It should_set_the_last_state_data_property = () =>
            {
                workflowData.LastState.ShouldBeTheSameAs(((ParamsDataStub)paramsData).StateName);
            };
    }
}