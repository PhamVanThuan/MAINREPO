using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Motivate.OnComplete
{
    [Subject("Activity => Application_in_Order => OnComplete")]
    internal class should_execute_pricing_for_risk : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;

        //static IApplicationManagement appMan;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;

            //appMan = An<IApplicationManagement>();
            common = An<ICommon>();

            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning)).Return(false);

            //domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Application_in_Order(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_call_pricing_for_risk = () =>
        {
            common.WasToldTo(x => x.PricingForRisk((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_pass = () =>
        {
            result.ShouldBeTrue();
        };
    }
}