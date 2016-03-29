using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Followup_Complete.OnEnter
{
    [Subject("State => Followup_Complete => OnEnter")]
    internal class when_followup_complete : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static List<string> list;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            result = false;
            list = new List<string>();
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Followup_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, list, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}