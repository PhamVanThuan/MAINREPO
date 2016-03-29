using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Review_Valuation_Required.OnComplete
{
    [Subject("Activity => Review_Valuation_Required => OnComplete")]
    internal class when_review_valuation_required : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon commonClient;
        private static string message;

        private Establish context = () =>
        {
            commonClient = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            commonClient.WhenToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Review_Valuation_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_if_valuation_is_in_progress = () =>
        {
            commonClient.WasToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey));
        };

        private It should_return_what_valuation_in_progress_return = () =>
        {
            result.ShouldBeTrue();
        };
    }
}