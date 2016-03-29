using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Decline_Quote.OnComplete
{
    [Subject("Activity => Decline_Quote => OnComplete")]
    internal class when_decline_quote : WorkflowSpecLife
    {
        private static bool result;
        private static string message;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            ((ParamsDataStub)paramsData).StateName = "Quote";
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline_Quote(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_decline_quote = () =>
        {
            client.WasToldTo(x => x.DeclineQuote(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_set_the_last_ntu_state_data_property_to_the_state_name_instance_data_property = () =>
        {
            workflowData.LastNTUState.ShouldMatch(paramsData.StateName);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}