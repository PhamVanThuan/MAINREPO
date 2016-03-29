using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Assign_Processor.OnStart
{
    [Subject("Activity => Assign_Processor => OnStart")]
    internal class when_assign_processor_where_does_not_have_valuation_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IValuations valuations;
        private static IWorkflowAssignment assignment;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            workflowData.RequireValuation = false;
            workflowData.IsFL = true;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            valuations = An<IValuations>();
            valuations.WhenToldTo(x => x.CheckValuationExistsRecentRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IValuations>(valuations);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Assign_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_require_valuation_data_property_to_true = () =>
        {
            workflowData.RequireValuation.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_continue_workflow_assignment = () =>
        {
            assignment.WasNotToldTo(x => x.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<long>()));
        };
    }
}