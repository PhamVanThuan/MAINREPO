using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU_Finalised.OnComplete
{
    [Subject("Activity => NTU_Finalised => OnComplete")]
    internal class when_ntu_finalised : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static ICommon commonClient;

        private Establish context = () =>
            {
                result = false;
                commonClient = An<ICommon>();
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_NTU_Finalised(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_update_the_offer_status_to_ntu = () =>
            {
                commonClient.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}