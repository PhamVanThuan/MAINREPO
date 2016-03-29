using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Disbursement_Incorrect.OnComplete
{
    [Subject("Activity => Disbursement_Complete => OnComplete")]
    internal class when_disbursement_is_incorrect : WorkflowSpecReadvancePayments
    {
        private static IWorkflowAssignment wfa;
        private static bool result;
        private static string message;
        private static List<string> roles = new List<string> { "FL Supervisor D" };

        private Establish context = () =>
            {
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Disbursement_Incorrect(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_deactivate_the_further_lending_supervisor = () =>
            {
                wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles, SAHL.Common.Globals.Process.Origination));
            };

        private It should_reactivate_or_round_robin_assign_a_further_lending_processor = () =>
            {
                wfa.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeysByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, null,
                    instanceData.ID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}