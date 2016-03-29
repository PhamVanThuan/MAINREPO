using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Further_Advance.OnComplete
{
    [Subject("Activity => Further_Advance => OnComplete")]
    internal class when_application_is_a_further_advance : WorkflowSpecReadvancePayments
    {
        private static List<string> roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", "RV Manager D", "RV Admin D" };
        private static string message;
        private static bool result;
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
            {
                ((InstanceDataStub)instanceData).ID = 1;
                result = false;
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Further_Advance(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_deactivate_further_lending_users_for_the_instance = () =>
            {
                wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey,
                    roles, SAHL.Common.Globals.Process.Origination));
            };

        private It should_reactivate_or_round_robin_assign_a_further_lending_processor = () =>
            {
                wfa.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157,
                    instanceData.ID, "Send Schedule", SAHL.Common.Globals.Process.Origination,
                    (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
            };
    }
}