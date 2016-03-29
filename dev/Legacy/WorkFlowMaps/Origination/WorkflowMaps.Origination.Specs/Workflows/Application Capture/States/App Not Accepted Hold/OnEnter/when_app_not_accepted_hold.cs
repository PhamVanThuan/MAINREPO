using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.App_Not_Accepted_Hold.OnEnter
{
    [Subject("State => App_Not_Accepted_Hold => OnEnter")]
    internal class when_app_not_accepted_hold : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            workflowData.ApplicationKey = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_App_Not_Accepted_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_offer_status_to_NTU = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}