using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reinstate_Decline.OnComplete
{
    [Subject("Activity => Reinstate_Decline => OnComplete")]
    internal class when_reinstate_decline : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment client;
        private static List<string> roles;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactive_user_for_instance_and_process = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_fl_supervisors = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, 155, instanceData.ID, "Reinstate Decline",
                                                                              SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}