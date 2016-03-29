using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Perform_Further_Valuation.OnStart
{
    [Subject("Activity => Perform_Further_Valuation => OnStart")]
    internal class when_require_valuation_and_check_if_can_further_valuation_true : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IValuations val;

        private Establish context = () =>
        {
            result = false;
            workflowData.RequireValuation = true;
            ((InstanceDataStub)instanceData).ID = 1;
            val = An<IValuations>();
            val.WhenToldTo(x => x.CheckIfCanPerformFurtherValuation(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IValuations>(val);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Perform_Further_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_if_can_perform_further_valuation = () =>
        {
            val.WasNotToldTo(x => x.CheckIfCanPerformFurtherValuation((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}