using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reinstate_NTU.OnComplete
{
    [Subject("Activity => Reinstate_NTU => OnComplete")]
    internal class when_reinstate_ntu_and_previous_state_is_rapid_decision : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static string ADUserName;
        private static IWorkflowAssignment client;
        private static List<string> roles;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            ADUserName = string.Empty;
            roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            workflowData.PreviousState = "rapid decision";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_fl_users = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey,
                                                                       roles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_fl_supervisors = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, 155, instanceData.ID, "Reinstate NTU",
                                                                              SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_get_latest_fl_processor_across_instances = () =>
        {
            client.WasNotToldTo(x => x.GetLatestUserAcrossInstances((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, 157, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_reassign_case_to_fl_processor = () =>
        {
            client.WasNotToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, ADUserName, 157, 857, "NTU"));
        };

        private It should_not_round_robin = () =>
        {
            client.WasNotToldTo(x => x.X2RoundRobinForPointerDescription((IDomainMessageCollection)messages, instanceData.ID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor,
                                                                         workflowData.ApplicationKey, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination));
        };
    }
}