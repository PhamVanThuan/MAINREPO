using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Override_Check.OnComplete
{
    [Subject("Activity => Override_Check => OnComplete")]
    internal class when_override_check_and_check_credit_override_rules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckCreditOverrideRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Override_Check(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_Check_CreditOverrideRules = () =>
        {
            appMan.WasToldTo(x => x.CheckCreditOverrideRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}