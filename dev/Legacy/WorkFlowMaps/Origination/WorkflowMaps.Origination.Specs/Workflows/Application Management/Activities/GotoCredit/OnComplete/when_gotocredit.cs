using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.GotoCredit.OnComplete
{
    [Subject("Activity => GotoCredit => OnComplete")]
    internal class when_gotocredit : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            dys = new List<string>(){"New Business Processor D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D"};
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_GotoCredit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactive_users_for_instance = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, dys, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}