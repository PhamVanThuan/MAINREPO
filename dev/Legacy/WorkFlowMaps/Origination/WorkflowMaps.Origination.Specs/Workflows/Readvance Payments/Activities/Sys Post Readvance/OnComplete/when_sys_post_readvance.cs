using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Post_Readvance.OnComplete
{
    [Subject("State => Sys_Post_Readvance => OnComplete")]
    internal class when_sys_post_readvance : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static ICommon common;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Sys_Post_Readvance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_get_applicationkey_from_source_instanceID = () =>
        {
            common.WasToldTo(x => x.GetApplicationKeyFromSourceInstanceID((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_reactive_user_or_roun_robin_for_oskey_by_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, 155, instanceData.ID, "Rapid Decision", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}