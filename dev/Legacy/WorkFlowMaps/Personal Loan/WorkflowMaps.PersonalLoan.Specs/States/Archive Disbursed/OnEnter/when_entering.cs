using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.PersonalLoan.Specs.States.Archive_Disbursed.OnEnter
{
    [Subject("State => Archive_Disbursed => OnEnter")]
    internal class when_entering : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static ICommon commonClient;

        private Establish context = () =>
        {
            commonClient = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Disbursed(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_offer_status_to_accepted = () =>
        {
            commonClient.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey,
                (int)SAHL.Common.Globals.OfferStatuses.Accepted, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}