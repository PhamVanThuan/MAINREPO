using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Next_Step.OnEnter
{
    [Subject("State => Next_Step => OnEnter")]
    internal class when_next_step_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = false;
            workflowRoles = new List<string> { "Branch Consultant D", "Branch Admin D", "Branch Manager D", "QA Administrator D" };
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Next_Step(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}