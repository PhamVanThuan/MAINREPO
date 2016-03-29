using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Require_Opt_Out_SuperLo.OnComplete
{
    [Subject("State => Require_Opt_Out_SuperLo => OnComplete")]
    internal class when_opt_out_super_lo_required : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static string message;
        private static List<string> workflowRoles;

        private Establish context = () =>
        {
            result = false;
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", };
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Require_Opt_Out_SuperLo(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_DeActive_Users_For_Instance_And_Process = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_ReactiveUser_Or_RoundRobin_For_OSKey_By_Process_supervisor = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, 155, instanceData.ID, "Require SuperLo Opt-Out", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor));
        };

        private It should_ReactiveUser_Or_RoundRobin_For_OSKey_By_Process_manager = () =>
       {
           workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Manager D", workflowData.ApplicationKey, 174, instanceData.ID, "Require SuperLo Opt-Out", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLManager));
       };

        private It should_return_true = () =>
       {
           result.ShouldBeTrue();
       };
    }
}