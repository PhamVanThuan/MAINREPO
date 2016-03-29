using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Override_Check.OnComplete
{
    [Subject("Activity => Override_Check => OnComplete")]
    internal class when_override_check : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            workflowData.IsFL = false;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckCreditOverrideRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Override_Check(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_override_check = () =>
        {
            appMan.WasToldTo(x => x.CheckCreditOverrideRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, true));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}