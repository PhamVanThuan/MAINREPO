using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Application_Reactivated.OnEnter
{
    [Subject("State => Application_Reactiviated => OnEnter")]
    internal class when_application_reactivated : WorkflowSpecApplicationCapture
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
            result = workflow.OnEnter_Application_Reactivated(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_offer_status_to_open = () =>
        {
            common.WhenToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}