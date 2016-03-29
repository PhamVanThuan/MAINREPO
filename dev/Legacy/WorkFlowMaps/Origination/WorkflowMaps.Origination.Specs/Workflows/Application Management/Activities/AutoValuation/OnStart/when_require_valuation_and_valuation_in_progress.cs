using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.AutoValuation.OnStart
{
    [Subject("Activity => AutoValuation => OnStart")]
    internal class when_require_valuation_and_valuation_in_progress : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon common;
        private static IValuations valuations;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            workflowData.RequireValuation = true;

            common = An<ICommon>();
            common.WhenToldTo(x => x.IsValuationInProgress(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>()))
                .Return(true);
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

        private It should_check_if_valuation_is_in_progress = () =>
        {
            common.WasToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_not_check_if_valuation_required = () =>
        {
            fl.WasNotToldTo(x => x.ValuationRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_not_check_valuation_exists_recent_rules = () =>
        {
            valuations.WasNotToldTo(x => x.CheckValuationExistsRecentRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };
    }
}