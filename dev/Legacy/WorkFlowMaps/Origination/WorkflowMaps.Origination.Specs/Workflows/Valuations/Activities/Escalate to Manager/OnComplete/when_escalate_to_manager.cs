using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Escalate_to_Manager.OnComplete
{
    [Subject("Activity => Escalate_to_Manager => OnComplete")]
    internal class when_escalate_to_manager : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment client;
        private static List<string> expectedDynamicRoles;

        private Establish context = () =>
        {
            expectedDynamicRoles = new List<string>
            {
                "Valuations Administrator D",
                "Valuations Manager D"
            };
            message = String.Empty;
            result = false;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Escalate_to_Manager(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, 0, expectedDynamicRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_user_or_round_robin = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Valuations Manager D", 0, 113, 0, "Manager Review",
                SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.ValautionsManager));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}