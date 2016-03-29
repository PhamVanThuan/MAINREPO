using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Perform_Further_Valuation.OnStart
{
    [Subject("Activity => Perform_Further_Valuation => OnStart")]
    internal class when_do_not_require_valuation : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IValuations val;

        private Establish context = () =>
        {
            result = true;
            workflowData.RequireValuation = false;
            val = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(val);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Perform_Further_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_if_can_perform_further_valuation = () =>
        {
            val.WasNotToldTo(x => x.CheckIfCanPerformFurtherValuation(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}