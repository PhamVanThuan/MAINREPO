using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Request_Lightstone_Valuation.OnComplete
{
    [Subject("State => Request_Lightstone_Valuation => OnComplete")]
    internal class when_request_lightstone_propertyVal_doesnt_exist : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static ICommon common;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            common.WhenToldTo(x => x.CheckPropertyExists((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Request_Lightstone_Valuation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_Not_Do_LightStone_Valuation_For_WorkFlow = () =>
        {
            common.WasNotToldTo(x => x.DoLightStoneValuationForWorkFlow((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}