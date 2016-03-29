using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Instruct_Attorney.OnComplete
{
    [Subject("Activity => Instruct_Attorney => OnComplete")]
    internal class when_instruct_attorney_and_check_instruct_attorney_rules_pass : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckInstructAttorneyRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Instruct_Attorney(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_instruct_attorney_rules = () =>
        {
            appMan.WasToldTo(x => x.CheckInstructAttorneyRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}