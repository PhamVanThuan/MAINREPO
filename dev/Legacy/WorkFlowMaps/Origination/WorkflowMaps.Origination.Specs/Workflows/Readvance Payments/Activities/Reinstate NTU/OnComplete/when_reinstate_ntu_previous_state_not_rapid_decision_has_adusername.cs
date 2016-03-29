using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Reinstate_NTU.OnComplete
{
    [Subject("Activity => Reinstate_NTU => OnComplete")]
    internal class when_reinstate_ntu_previous_state_not_rapid_decision_has_adusername : WorkflowSpecReadvancePayments
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
            ADUserName = @"SAHL\FLAppProcUser";
            workflowData.PreviousState = "Test";
            roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };

            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);

            client.WhenToldTo(x => x.GetLatestUserAcrossInstances(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>(),
                                                                  Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>(),
                                                                  Param.IsAny<SAHL.Common.Globals.Process>())).Return(ADUserName);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_fl_users = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles,
                                                                       SAHL.Common.Globals.Process.Origination));
        };

        private It should_get_latest_fl_processor_across_instances = () =>
        {
            client.WasToldTo(x => x.GetLatestUserAcrossInstances((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, 157, "FL Processor D", "NTU",
                                                                 SAHL.Common.Globals.Process.Origination));
        };

        private It should_reassign_case_to_fl_processor = () =>
        {
            client.WasToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, ADUserName, 157, 857, "NTU"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_reactivate_fl_users = () =>
        {
            client.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, 155, instanceData.ID, "Reinstate NTU",
                                                                                 SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor));
        };

        private It should_not_round_robin = () =>
        {
            client.WasNotToldTo(x => x.X2RoundRobinForPointerDescription((IDomainMessageCollection)messages, instanceData.ID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor,
                                                                         workflowData.ApplicationKey, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination));
        };
    }
}