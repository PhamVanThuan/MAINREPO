using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Continue_Sale.OnComplete
{
    [Subject("Activity => Continue_Sale => OnComplete")]
    internal class when_continuing_sale : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;
        private static string message = string.Empty;

        private Establish context = () =>
            {
                result = false;
                client = An<ILife>();
                domainServiceLoader.RegisterMockForType<ILife>(client);
                client.WhenToldTo(x => x.ContinueSale(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Continue_Sale(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_continue_the_sale = () =>
            {
                client.WasToldTo(x => x.ContinueSale(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
            };
    }
}