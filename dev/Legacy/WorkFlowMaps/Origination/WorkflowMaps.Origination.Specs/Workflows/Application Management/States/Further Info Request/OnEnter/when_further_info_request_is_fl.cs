using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Further_Info_Request.OnEnter
{
    [Subject("State => Further_Info_Request => OnEnter")]
    internal class when_further_info_request_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Further_Info_Request(instanceData, workflowData, paramsData, messages);
        };

        private It should_reactivate_users_or_round_robin_org_structure_157 = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID,
                                                                              "Further Info Request", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_not_reactivate_users_or_round_robin_org_structure_106 = () =>
        {
            client.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "New Business Processor D", workflowData.ApplicationKey, 106, instanceData.ID,
                                                                                 "Further Info Request", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}