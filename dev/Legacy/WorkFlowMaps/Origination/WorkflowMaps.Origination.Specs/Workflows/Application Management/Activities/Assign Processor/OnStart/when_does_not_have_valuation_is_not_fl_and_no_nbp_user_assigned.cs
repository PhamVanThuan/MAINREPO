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
    internal class when_assign_processor_where_does_not_have_valuation_is_not_fl_and_no_nbp_user_assigned : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static IValuations client;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            workflowData.RequireValuation = false;
            workflowData.IsFL = false;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<long>()))
                .Return(string.Empty);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            client = An<IValuations>();
            client.WhenToldTo(x => x.CheckValuationExistsRecentRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IValuations>(client);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Assign_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_valuation_exists_recent_rules = () =>
        {
            client.WasToldTo(x => x.CheckValuationExistsRecentRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_update_require_valuation_data_property_to_true = () =>
        {
            workflowData.RequireValuation.ShouldBeTrue();
        };

        private It should_get_user_who_worked_on_this_les_other_cases_for_dynamic_role = () =>
        {
            assignment.WasToldTo(x => x.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole((IDomainMessageCollection)messages, (int)SAHL.Common.Globals.OfferRoleTypes.NewBusinessProcessorD, workflowData.ApplicationKey, instanceData.ID));
        };

        private It should_rective_user_or_round_robin = () =>
        {
            assignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages,
                "New Business Processor D",
                workflowData.ApplicationKey,
                106,
                instanceData.ID,
                "Manage Application",
                SAHL.Common.Globals.Process.Origination,
                (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor));
        };

        private It should_update_assigned_user_in_idm = () =>
        {
            common.WasToldTo(x => x.UpdateAssignedUserInIDM((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                workflowData.IsFL,
                instanceData.ID,
                "Application Management"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}