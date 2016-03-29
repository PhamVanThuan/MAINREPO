using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Assign_at_QA.OnExit
{
    [Subject("States => Assign_at_QA => OnExit")]
    internal class when_assign_at_qa_and_is_not_switch_loan : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment client;
        private static List<int> oskeys;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = 7;
            workflowData.PreviousState = "Test";
            oskeys = new List<int> { 1007, 1008 };
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Assign_at_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_reactivate_qa_users = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeysByProcess((IDomainMessageCollection)messages, "QA Administrator D", workflowData.ApplicationKey, oskeys, instanceData.ID, "Assign at QA",
                                                                               SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.QAAdministrator));
        };

        private It should_set_workflow_data_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Assign at QA");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}