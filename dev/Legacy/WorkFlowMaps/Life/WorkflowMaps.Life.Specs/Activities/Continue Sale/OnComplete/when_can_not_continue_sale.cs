using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Continue_Sale.OnComplete
{
    [Subject("Activity => Continue_Sale => OnComplete")]
    internal class when_can_not_continue_sale : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;
        private static string message = string.Empty;

        private Establish context = () =>
        {
            result = true;
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
            client.WhenToldTo(x => x.ContinueSale(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Continue_Sale(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}