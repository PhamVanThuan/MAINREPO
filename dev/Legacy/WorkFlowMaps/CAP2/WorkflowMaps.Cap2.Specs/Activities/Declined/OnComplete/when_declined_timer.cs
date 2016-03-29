using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.Activities.Declined.OnComplete
{
    [Subject("Activity => Declined => OnComplete")]
    internal class when_declined_timer : WorkflowSpecCap2
    {
        private static ICap2 client;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            client = An<ICap2>();
            result = false;
            message = string.Empty;
            domainServiceLoader.RegisterMockForType<ICap2>(client);
            workflowData.CapOfferKey = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Declined(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_update_the_cap_status_to_offer_declined = () =>
        {
            client.WasToldTo(x => x.UpdateCapOfferStatus((IDomainMessageCollection)messages, workflowData.CapOfferKey, (int)SAHL.Common.Globals.CapStatuses.OfferDeclined));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}