using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Request_Lightstone_Valuation.OnComplete
{
    [Subject("Activity => Request_LighStone_Valuation => OnComplete")]
    internal class when_request_lightstone_valuation_and_rules_fail : WorkflowSpecCredit
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckPropertyExists(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            common.WhenToldTo(x => x.CheckLightStoneValuationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Request_Lightstone_Valuation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_if_property_exists = () =>
        {
            common.WasToldTo(x => x.CheckPropertyExists((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_check_the_lightstone_valuation_rules = () =>
        {
            common.WasToldTo(x => x.CheckLightStoneValuationRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_not_do_the_lightstone_valuation = () =>
        {
            common.WasNotToldTo(x => x.DoLightStoneValuationForWorkFlow((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}