using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.QA_Complete.OnComplete
{
    [Subject("Activity => QA_Complete => OnComplete")]
    internal class when_qa_complete_and_check_qa_complete_rules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement appMan;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckQACompleteRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_QA_Complete(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_qa_complete_rules = () =>
        {
            appMan.WasToldTo(x => x.CheckQACompleteRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_not_save_application_for_validation = () =>
        {
            appMan.WasNotToldTo(x => x.SaveApplication(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_not_perform_pricing_for_risk = () =>
        {
            common.WasNotToldTo(x => x.PricingForRisk(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}