using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._45_days.OnComplete
{
    [Subject("Activity => _30_day_timer => OnComplete")]
    internal class when_45_days : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon client;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            client = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_30_Day_Timer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_update_offer_status_to_NTU = () =>
        {
            client.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 4, -1));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}