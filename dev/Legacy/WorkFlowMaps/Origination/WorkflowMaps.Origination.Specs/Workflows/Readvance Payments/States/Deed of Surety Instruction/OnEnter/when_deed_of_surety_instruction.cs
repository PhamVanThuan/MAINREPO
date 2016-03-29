using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Deed_of_Surety_Instruction.OnEnter
{
    [Subject("State => Deed_Of_surety_Instruction => OnEnter")]
    internal class when_deed_of_surety_instruction : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            workflowRoles = new List<string> { "RV Admin D", "FL Processor D", "FL Manager D", "FL Supervisor D" };
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Deed_of_Surety_Instruction(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_user_or_round_robin_for_oskey_by_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "RV Admin D", workflowData.ApplicationKey, 595, instanceData.ID, "Deed of Surety Instruction", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.RVAdmin));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}