using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.NTU.OnComplete
{
    [Subject("Activity => NTU => OnComplete")]
    internal class when_previous_fl_processor_does_not_exist_across_instances : WorkflowSpecReadvancePayments
    {
        private static IWorkflowAssignment wfa;
        private static bool result;
        private static string message;
        private static List<string> roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };
        private static string aduser;

        private Establish context = () =>
            {
                aduser = string.Empty;
                result = false;
                wfa = An<IWorkflowAssignment>();
                wfa.WhenToldTo(x => x.GetLatestUserAcrossInstances(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>(),
                    Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Process>())).Return(aduser);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_NTU(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_deactivate_further_lending_users_for_instance = () =>
            {
                wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey,
                    roles, SAHL.Common.Globals.Process.Origination));
            };

        private It should_round_robin_assign_a_fl_processor = () =>
            {
                wfa.WasToldTo(x => x.X2RoundRobinForPointerDescription((IDomainMessageCollection)messages, instanceData.ID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor,
                    workflowData.ApplicationKey, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination));
            };

        private It should_not_reactivate_an_existing_processor = () =>
            {
                wfa.WasNotToldTo(x => x.ReassignCaseToUser(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>(),
                    Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<string>()));
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}