using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_Decline.OnComplete
{
    [Subject("Activity => Reinstate_Decline => OnComplete")]
    internal class when_reinstate_decline_where_is_not_fl_and_previous_state_is_qa : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;
        private static List<string> dys;
        private static List<int> osKeys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = false;
            workflowData.PreviousState = "QA";
            dys = new List<string>() { "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D",
                "New Business Processor D",
                "New Business Supervisor D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D" };
            osKeys = new List<int>() { 1007, 1008 };
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reinstate_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactive_users_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_qa_administrator_d_user_or_round_robin = () =>
        {
            assignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeysByProcess((IDomainMessageCollection)messages,
                "QA Administrator D",
                workflowData.ApplicationKey,
                osKeys,
                instanceData.ID,
                "Return To Sender From Decline",
                SAHL.Common.Globals.Process.Origination,
                (int)SAHL.Common.Globals.RoundRobinPointers.QAAdministrator));
        };

        private It should_not_reactive_new_business_processor_d_user_or_round_robin = () =>
        {
            assignment.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages,
                "New Business Processor D",
                workflowData.ApplicationKey,
                106,
                instanceData.ID,
                "Return To Sender From Decline",
                SAHL.Common.Globals.Process.Origination,
                (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor));
        };

        private It should_not_reactive_fl_processor_d_user_or_round_robin = () =>
        {
            assignment.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Return To Sender From Decline", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}