using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.SystemBackToCredit.OnStart
{
    [Subject("Activity => SystemBackToCredit => OnStart")]
    internal class when_systembacktocredit : WorkflowSpecApplicationManagement
    {
        private static List<string> expectedRoles;
        private static bool result;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            expectedRoles = new List<string>();
            expectedRoles.Add("Branch Consultant D");
            expectedRoles.Add("Branch Admin D");
            expectedRoles.Add("Branch Manager D");
            expectedRoles.Add("New Business Processor D");
            expectedRoles.Add("New Business Manager D");
            expectedRoles.Add("FL Processor D");
            expectedRoles.Add("FL Supervisor D");
            expectedRoles.Add("FL Manager D");
            result = false;

            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_SystemBackToCredit(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, expectedRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}