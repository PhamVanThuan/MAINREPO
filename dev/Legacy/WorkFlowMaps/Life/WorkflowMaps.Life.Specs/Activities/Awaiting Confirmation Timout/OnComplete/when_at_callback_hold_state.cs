using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Awaiting_Confirmation_Timout.OnComplete
{
    [Subject("Activity => Awaiting_Confirmation_Timout => OnComplete")]
    internal class when_at_callback_hold_state : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;
        private static string message;

        private Establish context = () =>
        {
            client = An<ILife>();
            result = true;
            ((ParamsDataStub)paramsData).StateName = "Callback Hold";
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Awaiting_Confirmation_Timout(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_not_complete_the_awaiting_confirmation_timeout = () =>
        {
            client.WasNotToldTo(x => x.AwaitingConfirmationTimeout(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };
    }
}