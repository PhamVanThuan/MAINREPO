using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.AutoValuation.OnStart
{
    [Subject("Activity => AutoValuation => OnStart")]
    internal class when_do_not_require_valuation : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon common;
        private static IValuations valuations;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            workflowData.RequireValuation = false;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            valuations = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(valuations);

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AutoValuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_not_check_if_valuation_is_in_progress = () =>
        {
            common.WasNotToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey));
        };
    }
}