using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Decline_Finalised.OnComplete
{
    [Subject("Activity => Decline_Finalised => OnComplete")]
    internal class when_declined_finalised : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static ICommon client;

        private Establish context = () =>
            {
                result = false;
                client = An<ICommon>();
                domainServiceLoader.RegisterMockForType<ICommon>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Decline_Finalised(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_update_offer_status_to_declined = () =>
            {
                client.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Declined, -1));
            };
    }
}